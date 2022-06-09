using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatsWindowUI : MonoBehaviour
{
    public Text itemNameText;
    public Image itemIconImage;

    [Header("Janela Das Status De Equipamento")]
    public GameObject weaponStats;
    public GameObject armorStats;

    [Header("Status Das Armas")]
    public Text physicalDamageText;
    public Text fireDamageText;
    public Text shockDamageText;
    public Text physicalAbsorptionText;
    public Text fireAbsorptionText;
    public Text shockAbsorptionText;

    [Header("Status Das Armaduras")]
    public Text armorPhysicalAbsorptionText;
    public Text armorFireAbsorptionText;
    public Text armorShockAbsorptionText;

    public void UpdateWeaponItemStats(WeaponItem weapon)
    {
        CloseAllStatWindows();

        if (weapon != null)
        {
            if (weapon.itemName != null)
            {
                itemNameText.text = weapon.itemName;
            }
            else
            {
                itemNameText.text = "";
            }

            if (weapon.itemIcon != null)
            {
                itemIconImage.gameObject.SetActive(true);
                itemIconImage.enabled = true;
                itemIconImage.sprite = weapon.itemIcon; 
            }
            else
            {
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
            }

            physicalDamageText.text = weapon.physicalDamage.ToString();
            physicalAbsorptionText.text = weapon.physicalDamageAbsorption.ToString();
            fireDamageText.text = weapon.fireDamage.ToString();
            fireAbsorptionText.text = weapon.fireDamageAbsorption.ToString();
            shockDamageText.text = weapon.shockDamage.ToString();
            shockAbsorptionText.text = weapon.shockDamageAbsorption.ToString();

            weaponStats.SetActive(true);
        }
        else
        {
            itemNameText.text = "";
            itemIconImage.gameObject.SetActive(false);
            itemIconImage.sprite = null;
            weaponStats.SetActive(false);
        }
    }

    public void UpdateArmorItemStats(EquipmentItem armor)
    {
        CloseAllStatWindows();

        if (armor != null)
        {
            if (armor.itemName != null)
            {
                itemNameText.text = armor.itemName;
            }
            else
            {
                itemNameText.text = "";
            }

            if (armor.itemIcon != null)
            {
                itemIconImage.gameObject.SetActive(true);
                itemIconImage.enabled = true;
                itemIconImage.sprite = armor.itemIcon; 
            }
            else
            {
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
            }

            armorPhysicalAbsorptionText.text = armor.physicalDefense.ToString();
            armorFireAbsorptionText.text = armor.fireDefense.ToString();
            armorShockAbsorptionText.text = armor.shockDefense.ToString();

            armorStats.SetActive(true);
        }
        else
        {
            itemNameText.text = "";
            itemIconImage.gameObject.SetActive(false);
            itemIconImage.sprite = null;
            armorStats.SetActive(false);
        }
    }

    private void CloseAllStatWindows()
    {
        weaponStats.SetActive(false);
        armorStats.SetActive(false);
    }

    //ATUALIZAR OS STATUS DOS CONSUMABLES
}
