using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInventory playerInventory;
    PlayerStats playerStats;

    [Header("Modificadores De Modelos De Equipamento")]
    HelmetModelChanger helmetModelChanger;
    TorsoModelChanger torsoModelChanger;
    FeetModelChanger feetModelChanger;
    LegsModelChanger legModelChanger;

    [Header("Modelos Nus Predefinidos")]
    public GameObject nakedHeadModel;
    public string nakedFeetModel;
    public string nakedLegsModel;
    
    public BlockingCollider blockingCollider;

    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerStats = GetComponentInParent<PlayerStats>();

        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        feetModelChanger = GetComponentInChildren<FeetModelChanger>();
        legModelChanger = GetComponentInChildren<LegsModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        helmetModelChanger.UnEquipAllHelmetModels();

        if (playerInventory.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            playerStats.physicalDamageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
            Debug.Log("Absorção Na Cabeça Foi De " + playerStats.physicalDamageAbsorptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStats.physicalDamageAbsorptionHead = 0;
        }

        torsoModelChanger.UnEquipAllTorsoModels();

        if (playerInventory.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            playerStats.physicalDamageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
            Debug.Log("Absorção No Tronco Foi De " + playerStats.physicalDamageAbsorptionBody + "%");
        }
        else
        {
            torsoModelChanger.UnEquipAllTorsoModels();
            playerStats.physicalDamageAbsorptionBody = 0;
        }

        feetModelChanger.UnEquipAllFeetModels();

        if (playerInventory.currentFeetEquipment != null)
        {
            feetModelChanger.EquipFeetModelByName(playerInventory.currentFeetEquipment.feetModelName);
            playerStats.physicalDamageAbsorptionFeet = playerInventory.currentFeetEquipment.physicalDefense;
            Debug.Log("Absorção Nos Pés Foi De " + playerStats.physicalDamageAbsorptionFeet + "%");
        }
        else
        {
            feetModelChanger.EquipFeetModelByName(nakedFeetModel);
            playerStats.physicalDamageAbsorptionFeet = 0;
        }

        legModelChanger.UnEquipAllLegModels();

        if (playerInventory.currentLegEquipment != null)
        {
            legModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.legModelName);
            playerStats.physicalDamageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
            Debug.Log("Absorção Nas Pernas Foi De " + playerStats.physicalDamageAbsorptionLegs + "%");
        }
        else
        {
            legModelChanger.EquipLegModelByName(nakedLegsModel);
            playerStats.physicalDamageAbsorptionLegs = 0;
        }
    }

    public void OpenBlockingCollider()
    {
        if (inputManager.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
