using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerStatsManager playerStatsManager;

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
        playerInventoryManager = GetComponentInParent<PlayerInventoryManager>();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();

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

        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            Debug.Log("Absorção Na Cabeça Foi De " + playerStatsManager.physicalDamageAbsorptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStatsManager.physicalDamageAbsorptionHead = 0;
        }

        torsoModelChanger.UnEquipAllTorsoModels();

        if (playerInventoryManager.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
            playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            Debug.Log("Absorção No Tronco Foi De " + playerStatsManager.physicalDamageAbsorptionBody + "%");
        }
        else
        {
            torsoModelChanger.UnEquipAllTorsoModels();
            playerStatsManager.physicalDamageAbsorptionBody = 0;
        }

        feetModelChanger.UnEquipAllFeetModels();

        if (playerInventoryManager.currentFeetEquipment != null)
        {
            feetModelChanger.EquipFeetModelByName(playerInventoryManager.currentFeetEquipment.feetModelName);
            playerStatsManager.physicalDamageAbsorptionFeet = playerInventoryManager.currentFeetEquipment.physicalDefense;
            Debug.Log("Absorção Nos Pés Foi De " + playerStatsManager.physicalDamageAbsorptionFeet + "%");
        }
        else
        {
            feetModelChanger.EquipFeetModelByName(nakedFeetModel);
            playerStatsManager.physicalDamageAbsorptionFeet = 0;
        }

        legModelChanger.UnEquipAllLegModels();

        if (playerInventoryManager.currentLegEquipment != null)
        {
            legModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.legModelName);
            playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
            Debug.Log("Absorção Nas Pernas Foi De " + playerStatsManager.physicalDamageAbsorptionLegs + "%");
        }
        else
        {
            legModelChanger.EquipLegModelByName(nakedLegsModel);
            playerStatsManager.physicalDamageAbsorptionLegs = 0;
        }
    }

    public void OpenBlockingCollider()
    {
        if (inputManager.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
