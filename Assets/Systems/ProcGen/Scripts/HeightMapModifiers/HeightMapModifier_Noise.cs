using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapModifier_Noise : BaseHeightMapModifier
{
    [SerializeField] float HeightDelta = 0f;
    [SerializeField] float XScale = 1f;
    [SerializeField] float YScale = 1f;
    [SerializeField] int NumPasses = 1;

    [SerializeField] float XScaleVariationPerPass = 2f;
    [SerializeField] float YScaleVariationPerPass = 2f;
    [SerializeField] float HeightDeltaVariationPerPass = 0.5f;

    public override void Execute(ProcGenConfigSO globalConfig, int mapResolution, float[,] heightMap, Vector3 heightmapScale, byte[,] biomeMap = null, int biomeIndex = -1, BiomeConfigSO biome = null)
    {
        float workingXScale = XScale;
        float workingYScale = YScale;
        float WorkingHeightDelta = HeightDelta;

        for (int pass = 0; pass < NumPasses; ++pass)
        {
            for (int y = 0; y < mapResolution; ++y)
            {
                for (int x = 0; x < mapResolution; ++x)
                {
                    // passar se tivermos um bioma e isto não ser o nosso bioma
                    if (biomeIndex >= 0 && biomeMap[x, y] != biomeIndex)
                        continue;

                    float noiseValue = (Mathf.PerlinNoise(x * workingXScale, y * workingYScale) * 2f) - 1f;

                    // calcula a nova altura
                    float newHeight = heightMap[x, y] + (noiseValue * WorkingHeightDelta / heightmapScale.y);

                    // misturar baseado na força
                    heightMap[x, y] = Mathf.Lerp(heightMap[x, y], newHeight, Strength);
                }
            }

            // atualiza parâmetros
            workingXScale *= XScaleVariationPerPass;
            workingYScale *= YScaleVariationPerPass;
            WorkingHeightDelta *= HeightDeltaVariationPerPass;
        }
    }
}
