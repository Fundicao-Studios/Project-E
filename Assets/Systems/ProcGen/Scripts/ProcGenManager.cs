using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif // UNITY_EDITOR

public class ProcGenData
{
    public byte[,] BiomeMap_LowResolution;
    public float[,] BiomeStrengths_LowResolution;

    public byte[,] BiomeMap;
    public float[,] BiomeStrengths;

    public float[,] SlopeMap;
}

public class ProcGenManager : MonoBehaviour
{
    [SerializeField] ProcGenConfigSO Config;
    [SerializeField] Terrain TargetTerrain;
    [SerializeField] Texture2D Output_BiomeMap;
    [SerializeField] Texture2D Output_SlopeMap;
    [SerializeField] PathdataManager navMesh;

    ProcGenData WorkingData;

    [Header("Debugging")]
    [SerializeField] bool DEBUG_TurnOffObjectPlacers = false;

    Dictionary<TextureConfig, int> BiomeTextureToTerrainLayerIndex = new Dictionary<TextureConfig, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AsyncRegenerateWorld(System.Action<int, int, string> reportStatusFn = null)
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Regenerate World");
#endif // UNITY_EDITOR
        WorkingData = new ProcGenData();

        // passar a resolução do mapa para o cache
        int mapResolution = TargetTerrain.terrainData.heightmapResolution;
        int alphaMapResolution = TargetTerrain.terrainData.alphamapResolution;

        if (reportStatusFn != null) reportStatusFn.Invoke(1, 8, "Começar Geração");
        yield return new WaitForSeconds(1f);

        // limpar todos os objetos spawnados anteriormente
        for (int childIndex = transform.childCount - 1; childIndex >= 0; --childIndex)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                Destroy(transform.GetChild(childIndex).gameObject);
            else
                Undo.DestroyObjectImmediate(transform.GetChild(childIndex).gameObject);
#else
            Destroy(transform.GetChild(childIndex).gameObject);
#endif // UNITY_EDITOR
        }

        if (reportStatusFn != null) reportStatusFn.Invoke(2, 8, "A construir o mapa de texturas");
        yield return new WaitForSeconds(1f);

        // Gerar o mapa de texturas
        Perform_GenerateTextureMapping();

        if (reportStatusFn != null) reportStatusFn.Invoke(3, 8, "A construir o mapa de texturas de baixa resolução");
        yield return new WaitForSeconds(1f);

        // gerar um mapa de biomas de baixa resolução
        Perform_BiomeGeneration_LowResolution((int)Config.BiomeMapResolution);

        if (reportStatusFn != null) reportStatusFn.Invoke(4, 8, "A construir o mapa de biomas de alta resolução");
        yield return new WaitForSeconds(1f);

        // gerar um mapa de biomas de alta resolução
        Perform_BiomeGeneration_HighResolution((int)Config.BiomeMapResolution, mapResolution);

        if (reportStatusFn != null) reportStatusFn.Invoke(5, 8, "A modificar as alturas");
        yield return new WaitForSeconds(1f);

        // atualiza as alturas do terreno
        Perform_HeightMapModification(mapResolution, alphaMapResolution);

        if (reportStatusFn != null) reportStatusFn.Invoke(6, 8, "A pintar o terreno");
        yield return new WaitForSeconds(1f);

        // pintar o terreno
        Perform_TerrainPainting(mapResolution, alphaMapResolution);

        if (reportStatusFn != null) reportStatusFn.Invoke(7, 8, "A posicionar objetos");
        yield return new WaitForSeconds(1f);

        // posiciona os objetos
        Perform_ObjectPlacement(mapResolution, alphaMapResolution);

        if (reportStatusFn != null) reportStatusFn.Invoke(8, 8, "A gerar a PathData");
        yield return new WaitForSeconds(1f);

        // gera a pathdata
        navMesh.Internal_BuildPathdata();

        if (reportStatusFn != null) reportStatusFn.Invoke(8, 8, "Geração Concluída");
    }

    void Perform_GenerateTextureMapping()
    {
        BiomeTextureToTerrainLayerIndex.Clear();

        // construir uma lista de todas as texturas
        List<TextureConfig> allTextures = new List<TextureConfig>();
        foreach(var biomeMetaData in Config.Biomes)
        {
            List<TextureConfig> biomeTextures = biomeMetaData.Biome.RetrieveTextures();

            if (biomeTextures == null || biomeTextures.Count == 0)
                continue;

            allTextures.AddRange(biomeTextures);
        }

        if (Config.PaintingPostProcessingModifier != null)
        {
            // extrair todas as texturas de todos os pintores
            BaseTexturePainter[] allPainters = Config.PaintingPostProcessingModifier.GetComponents<BaseTexturePainter>();
            foreach (var painter in allPainters)
            {
                var painterTextures = painter.RetrieveTextures();

                if (painterTextures == null || painterTextures.Count == 0)
                    continue;

                allTextures.AddRange(painterTextures);
            }
        }

        // filtrar todos os elementos duplicados
        allTextures = allTextures.Distinct().ToList();

        // passar por todas as configurações de texturas
        int layerIndex = 0;
        foreach(var TextureConfig in allTextures)
        {
            BiomeTextureToTerrainLayerIndex[TextureConfig] = layerIndex;
            ++layerIndex;
        }
    }

#if UNITY_EDITOR
    public void RegenerateTextures()
    {
        Perform_LayerSetup();
    }

    void Perform_LayerSetup()
    {
        // remove todas as camadas existentes
        if (TargetTerrain.terrainData.terrainLayers != null || TargetTerrain.terrainData.terrainLayers.Length > 0)
        {
            Undo.RecordObject(TargetTerrain, "A remover camadas anteriores");

            // constroi a lista de asset paths para cada camada 
            List<string> layersToDelete = new List<string>();
            foreach(var layer in TargetTerrain.terrainData.terrainLayers)
            {
                if (layer == null)
                    continue;
                
                layersToDelete.Add(AssetDatabase.GetAssetPath(layer.GetInstanceID()));
            }

            // remove todas as ligações às camadas
            TargetTerrain.terrainData.terrainLayers = null;

            // remove cada camada
            foreach(var layerFile in layersToDelete)
            {
                if (string.IsNullOrEmpty(layerFile))
                    continue;

                AssetDatabase.DeleteAsset(layerFile);
            }

            Undo.FlushUndoRecordObjects();
        }

        string scenePath = System.IO.Path.GetDirectoryName(SceneManager.GetActiveScene().path);

        Perform_GenerateTextureMapping();

        // gerar todas as camadas
        int numLayers = BiomeTextureToTerrainLayerIndex.Count;
        List<TerrainLayer> newLayers = new List<TerrainLayer>();

        // pre-alocar as camadas
        for (int layerIndex = 0; layerIndex < numLayers; ++layerIndex)
        {
            newLayers.Add(new TerrainLayer());
        }

        // passar pelo mapa de texturas
        foreach(var textureMappingEntry in BiomeTextureToTerrainLayerIndex)
        {
            var textureConfig = textureMappingEntry.Key;
            var textureLayerIndex = textureMappingEntry.Value;
            var textureLayer = newLayers[textureLayerIndex];

            // configurar as texturas das camadas do terreno
            textureLayer.diffuseTexture = textureConfig.Diffuse;
            textureLayer.normalMapTexture = textureConfig.NormalMap;
            
            // salvar como um asset
            string layerPath = System.IO.Path.Combine(scenePath, "Layer_" + textureLayerIndex);
            AssetDatabase.CreateAsset(textureLayer, layerPath);
        }

        Undo.RecordObject(TargetTerrain.terrainData, "A atualizar as camadas do terreno");
        TargetTerrain.terrainData.terrainLayers = newLayers.ToArray();
    }
#endif //UNITY_EDITOR

    void Perform_BiomeGeneration_LowResolution(int mapResolution)
    {
        // alocar o mapa de biomas e da sua relevância
        WorkingData.BiomeMap_LowResolution = new byte[mapResolution, mapResolution];
        WorkingData.BiomeStrengths_LowResolution = new float[mapResolution, mapResolution];

        // definir o espaço dos biomas
        int numSeedPoints = Mathf.FloorToInt(mapResolution * mapResolution * Config.BiomeSeedPointDensity);
        List<byte> biomesToSpawn = new List<byte>();

        // preencher os biomas a spawnar baseado nos seus pesos
        float totalBiomeWeighting = Config.TotalWeighting;
        for (int biomeIndex = 0; biomeIndex < Config.NumBiomes; ++biomeIndex)
        {
            int numEntries = Mathf.RoundToInt(numSeedPoints * Config.Biomes[biomeIndex].Weighting / totalBiomeWeighting);

            for (int entryIndex = 0; entryIndex < numEntries; ++entryIndex)
            {
                biomesToSpawn.Add((byte)biomeIndex);
            }
        }

        // spawnar os biomas individuais
        while (biomesToSpawn.Count > 0)
        {
            // escolher uma seed aleatória
            int seedPointIndex = Random.Range(0, biomesToSpawn.Count);

            // extrair o índice do bioma
            byte biomeIndex = biomesToSpawn[seedPointIndex];

            // remove o ponto da seed
            biomesToSpawn.RemoveAt(seedPointIndex);

            Perform_SpawnIndividualBiome(biomeIndex, mapResolution);
        }

#if UNITY_EDITOR
        // salvar o mapa de biomas
        Texture2D biomeMap = new Texture2D(mapResolution, mapResolution, TextureFormat.RGB24, false);
        for (int y = 0; y < mapResolution; ++y)
        {
            for (int x = 0; x < mapResolution; ++x)
            {
                float hue = ((float)WorkingData.BiomeMap_LowResolution[x, y] / (float)Config.NumBiomes);

                biomeMap.SetPixel(x,y, Color.HSVToRGB(hue, 0.75f, 0.75f));
            }
        }
        biomeMap.Apply();

        System.IO.File.WriteAllBytes("BiomeMap_LowResolution.png", biomeMap.EncodeToPNG());
#endif // UNITY_EDITOR
    }

    Vector2Int[] NeighbourOffsets = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
    };

    /*
    Usar geração baseada em "Ooze" - https://www.procjam.com/tutorials/en/ooze/
    */
    void Perform_SpawnIndividualBiome(byte biomeIndex, int mapResolution)
    {
        // passar as configurações do bioma para o cache
        BiomeConfigSO biomeConfig = Config.Biomes[biomeIndex].Biome;

        // escolher o local de spawn
        Vector2Int spawnLocation = new Vector2Int(Random.Range(0, mapResolution), Random.Range(0, mapResolution));

        // escolher a intensidade inicial
        float startIntensity = Random.Range(biomeConfig.MinIntensity, biomeConfig.MaxIntensity);

        // definir a lista de trabalho
        Queue<Vector2Int> workingList = new Queue<Vector2Int>();
        workingList.Enqueue(spawnLocation);

        // definir o mapa visitado e o mapa de intensidade alvo
        bool[,] visited = new bool[mapResolution, mapResolution];
        float[,] targetIntensity = new float[mapResolution, mapResolution];

        // definir a intensidade inicial
        targetIntensity[spawnLocation.x, spawnLocation.y] = startIntensity;

        // let the oozing begin!
        while (workingList.Count > 0)
        {
            Vector2Int workingLocation = workingList.Dequeue();

            // definir o bioma
            WorkingData.BiomeMap_LowResolution[workingLocation.x, workingLocation.y] = biomeIndex;
            visited[workingLocation.x, workingLocation.y] = true;
            WorkingData.BiomeStrengths_LowResolution[workingLocation.x, workingLocation.y] = targetIntensity[workingLocation.x, workingLocation.y];

            // atravessar os vizinhos
            for (int neighbourIndex = 0; neighbourIndex < NeighbourOffsets.Length; ++neighbourIndex)
            {
                Vector2Int neighbourLocation = workingLocation + NeighbourOffsets[neighbourIndex];

                // passar à frente se for inválido
                if (neighbourLocation.x < 0 || neighbourLocation.y < 0 || neighbourLocation.x >= mapResolution || neighbourLocation.y >= mapResolution)
                    continue;

                // passar se foi visitado
                if (visited[neighbourLocation.x, neighbourLocation.y])
                    continue;

                // definir como visitada
                visited[neighbourLocation.x, neighbourLocation.y] = true;

                // trabalhar e guardar a intensidade do vizinho
                float decayAmount = Random.Range(biomeConfig.MinDecayRate, biomeConfig.MaxDecayRate) * NeighbourOffsets[neighbourIndex].magnitude;
                float neighbourStrength = targetIntensity[workingLocation.x, workingLocation.y] - decayAmount;
                targetIntensity[neighbourLocation.x, neighbourLocation.y] = neighbourStrength;

                // se a intensidade for demasiado baixa - parar
                if (neighbourStrength <= 0)
                {
                    continue;
                }

                workingList.Enqueue(neighbourLocation);
            }
        }
    }

    byte CalculateHighResBiomeIndex(int lowResMapSize, int lowResX, int lowResY, float fractionX, float fractionY)
    {
        float A = WorkingData.BiomeMap_LowResolution[lowResX,     lowResY];
        float B = (lowResX + 1) < lowResMapSize ? WorkingData.BiomeMap_LowResolution[lowResX + 1, lowResY] : A;
        float C = (lowResY + 1) < lowResMapSize ? WorkingData.BiomeMap_LowResolution[lowResX,     lowResY + 1] : A;
        float D = 0;

        if ((lowResX + 1) >= lowResMapSize)
            D = C;
        else if ((lowResY + 1) >= lowResMapSize)
            D = B;
        else
            D = WorkingData.BiomeMap_LowResolution[lowResX + 1, lowResY + 1];

        // fazer a filtragem bilinear
        float filteredIndex = A * (1 - fractionX) * (1 - fractionY) + B * fractionX * (1 - fractionY) *
                              C * fractionY * (1 - fractionX) + D * fractionX * fractionY;

        // construir um array dos possíveis biomas baseado nos valores usados para interpolar
        float[] candidateBiomes = new float[] {A, B, C, D};

        // encontra o bioma vizinho mais próximo do bioma interpolado
        float bestBiome = -1f;
        float bestDelta = float.MaxValue;
        for (int biomeIndex = 0; biomeIndex < candidateBiomes.Length; ++biomeIndex)
        {
            float delta = Mathf.Abs(filteredIndex - candidateBiomes[biomeIndex]);

            if (delta < bestDelta)
            {
                bestDelta = delta;
                bestBiome = candidateBiomes[biomeIndex];
            }
        }

        return (byte)Mathf.RoundToInt(bestBiome);
    }

    void Perform_BiomeGeneration_HighResolution(int lowResMapSize, int highResMapSize)
    {
        // alocar o mapa de biomas e da sua relevância
        WorkingData.BiomeMap = new byte[highResMapSize, highResMapSize];
        WorkingData.BiomeStrengths = new float[highResMapSize, highResMapSize];

        // calcula a escala do mapa
        float mapScale = (float)lowResMapSize / (float)highResMapSize;
        
        // calcula a resolução do mapa de alta resolução
        for (int y = 0; y < highResMapSize; ++y)
        {
            int lowResY = Mathf.FloorToInt(y * mapScale);
            float yFraction = y * mapScale - lowResY;

            for(int x = 0; x < highResMapSize; ++x)
            {
                int lowResX = Mathf.FloorToInt(x * mapScale);
                float xFraction = x * mapScale - lowResX;

                WorkingData.BiomeMap[x, y] = CalculateHighResBiomeIndex(lowResMapSize, lowResX, lowResY, xFraction, yFraction);

                // isto não iria fazer interpolação - ex. baseada em pontos
                //BiomeMap[x, y] = BiomeMap_LowResolution[lowResX, lowResY];
            }
        }

#if UNITY_EDITOR
        // salvar o mapa de biomas e converter numa textura
        Texture2D tempTexture = new Texture2D(highResMapSize, highResMapSize, TextureFormat.RGB24, false);
        for (int y = 0; y < highResMapSize; ++y)
        {
            for (int x = 0; x < highResMapSize; ++x)
            {
                float hue = ((float)WorkingData.BiomeMap[x, y] / (float)Config.NumBiomes);

                tempTexture.SetPixel(x,y, Color.HSVToRGB(hue, 0.75f, 0.75f));
            }
        }
        tempTexture.Apply();

        string outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(SceneManager.GetActiveScene().path), "ProcGen_BiomeMap.png");
        System.IO.File.Delete(outputPath);
        System.IO.File.WriteAllBytes(outputPath, tempTexture.EncodeToPNG());

        Debug.Log(outputPath);
        Output_BiomeMap = AssetDatabase.LoadAssetAtPath<Texture2D>(outputPath);
#endif // UNITY_EDITOR
    }

    void Perform_HeightMapModification(int mapResolution, int alphaMapResolution)
    {
        float[,] heightMap = TargetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);

        // executa todos os modificadores de altura iniciais
        if (Config.InitialHeightModifier != null)
        {
            BaseHeightMapModifier[] modifiers = Config.InitialHeightModifier.GetComponents<BaseHeightMapModifier>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(Config, mapResolution, heightMap, TargetTerrain.terrainData.heightmapScale);
            }
        }

        // correr a geração do heightmap para cada bioma
        for (int biomeIndex = 0; biomeIndex < Config.NumBiomes; ++biomeIndex)
        {
            var biome = Config.Biomes[biomeIndex].Biome;
            if (biome.HeightModifier == null)
                continue;
                
            BaseHeightMapModifier[] modifiers = biome.HeightModifier.GetComponents<BaseHeightMapModifier>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(Config, mapResolution, heightMap, TargetTerrain.terrainData.heightmapScale, WorkingData.BiomeMap, biomeIndex, biome);
            }
        }

        // executa todos os modificadores de altura de post processing
        if (Config.HeightPostProcessingModifier != null)
        {
            BaseHeightMapModifier[] modifiers = Config.HeightPostProcessingModifier.GetComponents<BaseHeightMapModifier>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(Config, mapResolution, heightMap, TargetTerrain.terrainData.heightmapScale);
            }
        }

        TargetTerrain.terrainData.SetHeights(0, 0, heightMap);

        // gerar o mapa de inclinações
        WorkingData.SlopeMap = new float[alphaMapResolution, alphaMapResolution];
        for (int y = 0; y < alphaMapResolution; ++y)
        {
            for (int x = 0; x < alphaMapResolution; ++x)
            {
                 WorkingData.SlopeMap[x, y] = TargetTerrain.terrainData.GetInterpolatedNormal((float) x / alphaMapResolution, (float) y / alphaMapResolution).y;
            }
        }

        // converter o mapa de inclinações para uma textura
        Texture2D tempTexture = new Texture2D(alphaMapResolution, alphaMapResolution, TextureFormat.RGB24, false);
        for (int y = 0; y < alphaMapResolution; ++y)
        {
            for (int x = 0; x < alphaMapResolution; ++x)
            {
                float intensity = WorkingData.SlopeMap[x, y];

                tempTexture.SetPixel(x, y, new Color(intensity, intensity, intensity));
            }
        }
        tempTexture.Apply();

        string outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.persistentDataPath), "ProcGen_SlopeMap.png");
        System.IO.File.Delete(outputPath);
        System.IO.File.WriteAllBytes(outputPath, tempTexture.EncodeToPNG());

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif // UNITY_EDITOR

        byte[] bytes = System.IO.File.ReadAllBytes(outputPath);
        Texture2D temTexture = new Texture2D(1,1);
        temTexture.LoadImage(bytes);
        Output_SlopeMap = temTexture;
    }

    public int GetLayerForTexture(TextureConfig textureConfig)
    {
        return BiomeTextureToTerrainLayerIndex[textureConfig];
    }

    void Perform_TerrainPainting(int mapResolution, int alphaMapResolution)
    {
        float[,] heightMap = TargetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        float[,,] alphaMaps = TargetTerrain.terrainData.GetAlphamaps(0, 0, alphaMapResolution, alphaMapResolution); 

        // zerar todas as camadas
        for (int y = 0; y < alphaMapResolution; ++y)
        {
            for (int x = 0; x < alphaMapResolution; ++x)
            {
                for (int layerIndex = 0; layerIndex < TargetTerrain.terrainData.alphamapLayers; ++layerIndex)
                {
                    alphaMaps[x, y, layerIndex] = 0;
                }    
            }
        }

        // correr o pintor de terreno para cada bioma
        for (int biomeIndex = 0; biomeIndex < Config.NumBiomes; ++biomeIndex)
        {
            var biome = Config.Biomes[biomeIndex].Biome;
            if (biome.TerrainPainter == null)
                continue;
                
            BaseTexturePainter[] modifiers = biome.TerrainPainter.GetComponents<BaseTexturePainter>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(this, mapResolution, heightMap, TargetTerrain.terrainData.heightmapScale, WorkingData.SlopeMap, alphaMaps, alphaMapResolution, WorkingData.BiomeMap, biomeIndex, biome);
            }
        }

        // correr o post processing das texturas
        if (Config.PaintingPostProcessingModifier != null)
        {
            BaseTexturePainter[] modifiers = Config.PaintingPostProcessingModifier.GetComponents<BaseTexturePainter>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(this, mapResolution, heightMap, TargetTerrain.terrainData.heightmapScale, WorkingData.SlopeMap, alphaMaps, alphaMapResolution);
            }
        }

        TargetTerrain.terrainData.SetAlphamaps(0, 0, alphaMaps);   
    }

    void Perform_ObjectPlacement(int mapResolution, int alphaMapResolution)
    {
        if (DEBUG_TurnOffObjectPlacers)
            return;

        float[,] heightMap = TargetTerrain.terrainData.GetHeights(0, 0, mapResolution, mapResolution);
        float[,,] alphaMaps = TargetTerrain.terrainData.GetAlphamaps(0, 0, alphaMapResolution, alphaMapResolution);

        // correr o colocador de objetos para cada bioma
        for (int biomeIndex = 0; biomeIndex < Config.NumBiomes; ++biomeIndex)
        {
            var biome = Config.Biomes[biomeIndex].Biome;
            if (biome.ObjectPlacer == null)
                continue;
                
            BaseObjectPlacer[] modifiers = biome.ObjectPlacer.GetComponents<BaseObjectPlacer>();

            foreach(var modifier in modifiers)
            {
                modifier.Execute(Config, transform, mapResolution, heightMap, TargetTerrain.terrainData.heightmapScale, WorkingData.SlopeMap, alphaMaps, alphaMapResolution, WorkingData.BiomeMap, biomeIndex, biome);
            }
        }
    }
}
