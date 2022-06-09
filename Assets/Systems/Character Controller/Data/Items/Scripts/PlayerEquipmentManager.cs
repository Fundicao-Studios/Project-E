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
        EquipAllEquipmentModels();
    }

    public void EquipAllEquipmentModels()
    {
        helmetModelChanger.UnEquipAllHelmetModels();

        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            playerStatsManager.fireDamageAbsorptionBody = playerInventoryManager.currentHelmetEquipment.fireDefense;
            playerStatsManager.shockDamageAbsorptionBody = playerInventoryManager.currentHelmetEquipment.shockDefense;
            //Debug.Log("Absorção Na Cabeça Foi De " + playerStatsManager.physicalDamageAbsorptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStatsManager.physicalDamageAbsorptionHead = 0;
            playerStatsManager.fireDamageAbsorptionHead = 0;
            playerStatsManager.shockDamageAbsorptionHead = 0;
        }

        torsoModelChanger.UnEquipAllTorsoModels();

        if (playerInventoryManager.currentBodyEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentBodyEquipment.torsoModelName);
            playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentBodyEquipment.physicalDefense;
            playerStatsManager.fireDamageAbsorptionBody = playerInventoryManager.currentBodyEquipment.fireDefense;
            playerStatsManager.shockDamageAbsorptionBody = playerInventoryManager.currentBodyEquipment.shockDefense;
            //Debug.Log("Absorção No Tronco Foi De " + playerStatsManager.physicalDamageAbsorptionBody + "%");
        }
        else
        {
            torsoModelChanger.UnEquipAllTorsoModels();
            playerStatsManager.physicalDamageAbsorptionBody = 0;
            playerStatsManager.fireDamageAbsorptionBody = 0;
            playerStatsManager.shockDamageAbsorptionBody = 0;
        }

        feetModelChanger.UnEquipAllFeetModels();

        if (playerInventoryManager.currentFeetEquipment != null)
        {
            feetModelChanger.EquipFeetModelByName(playerInventoryManager.currentFeetEquipment.feetModelName);
            playerStatsManager.physicalDamageAbsorptionFeet = playerInventoryManager.currentFeetEquipment.physicalDefense;
            playerStatsManager.fireDamageAbsorptionFeet = playerInventoryManager.currentFeetEquipment.fireDefense;
            playerStatsManager.shockDamageAbsorptionFeet = playerInventoryManager.currentFeetEquipment.shockDefense;
            //Debug.Log("Absorção Nos Pés Foi De " + playerStatsManager.physicalDamageAbsorptionFeet + "%");
        }
        else
        {
            feetModelChanger.EquipFeetModelByName(nakedFeetModel);
            playerStatsManager.physicalDamageAbsorptionFeet = 0;
            playerStatsManager.fireDamageAbsorptionFeet = 0;
            playerStatsManager.shockDamageAbsorptionFeet = 0;
        }

        legModelChanger.UnEquipAllLegModels();

        if (playerInventoryManager.currentLegEquipment != null)
        {
            legModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.legModelName);
            playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
            playerStatsManager.fireDamageAbsorptionLegs = playerInventoryManager.currentFeetEquipment.fireDefense;
            playerStatsManager.shockDamageAbsorptionLegs = playerInventoryManager.currentFeetEquipment.shockDefense;
            //Debug.Log("Absorção Nas Pernas Foi De " + playerStatsManager.physicalDamageAbsorptionLegs + "%");
        }
        else
        {
            legModelChanger.EquipLegModelByName(nakedLegsModel);
            playerStatsManager.physicalDamageAbsorptionLegs = 0;
            playerStatsManager.fireDamageAbsorptionLegs = 0;
            playerStatsManager.shockDamageAbsorptionLegs = 0;
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
