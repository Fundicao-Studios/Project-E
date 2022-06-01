using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public ConsumableItem currentConsumable;
    public GameObject currentConsumableModel;

    public void UnloadConsumable()
    {
        if (currentConsumable != null)
        {
            currentConsumableModel.SetActive(false);
        }
    }

    public void UnloadConsumableAndDestroy()
    {
        if (currentConsumable != null)
        {
            Destroy(currentConsumableModel);
        }
    }

    public void LoadConsumableModel(ConsumableItem consumableItem)
    {
        UnloadConsumableAndDestroy();

        if (consumableItem == null)
        {
            UnloadConsumable();
            return;
        }

        GameObject model = Instantiate(consumableItem.itemModel) as GameObject;
        if(model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            //model.transform.localScale = Vector3.one;
        }

        currentConsumableModel = model;
    }
}
