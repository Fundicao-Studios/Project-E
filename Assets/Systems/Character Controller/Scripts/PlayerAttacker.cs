using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputManager.comboFlag)
        {
            animatorManager.anim.SetBool("canDoCombo", false);

            if (lastAttack == weapon.oh_light_attack_01)
            {
                animatorManager.PlayTargetAnimation(weapon.oh_light_attack_02, true);
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                animatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twoHandFlag)
        {
            animatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true);
            lastAttack = weapon.th_light_attack_01;
        }
        else
        {
            animatorManager.PlayTargetAnimation(weapon.oh_light_attack_01, true);
            lastAttack = weapon.oh_light_attack_01;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twoHandFlag)
        {

        }
        else
        {
            animatorManager.PlayTargetAnimation(weapon.oh_heavy_attack_01, true);
            lastAttack = weapon.oh_heavy_attack_01;
        }
    }
}
