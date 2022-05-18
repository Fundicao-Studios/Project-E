using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetModelChanger : MonoBehaviour
{
    public List<GameObject> feetModels;

    private void Awake()
    {
        GetAllFeetModels();
    }

    private void GetAllFeetModels()
    {
        int childrenGameObjects = transform.childCount;

        for (int i = 0; i < childrenGameObjects; i++)
        {
            feetModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllFeetModels()
    {
        foreach (GameObject feetModel in feetModels)
        {
            feetModel.SetActive(false);
        }
    }

    public void EquipFeetModelByName(string feetName)
    {
        for (int i = 0; i < feetModels.Count; i++)
        {
            if (feetModels[i].name == feetName)
            {
                feetModels[i].SetActive(true);
            }
        }
    }
}
