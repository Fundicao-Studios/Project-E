using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHeardSoundCategory
{
    EFootstep,
    EJump
}

public class HearingManager : MonoBehaviour
{
    public static HearingManager Instance { get; private set; } = null;

    public List<HearingSensor> AllSensors { get; private set; } = new List<HearingSensor>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Múltiplos HearingManagers encontrados. Destruindo " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register(HearingSensor sensor)
    {
        AllSensors.Add(sensor);
    }

    public void Deregister(HearingSensor sensor)
    {
        AllSensors.Remove(sensor);
    }

    public void OnSoundEmitted(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        // notificar todos os sensores
        foreach(var sensor in AllSensors)
        {
            sensor.OnHeardSound(source, location, category, intensity);
        }
    }
}
