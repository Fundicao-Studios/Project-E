using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    public string lastAttack;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
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
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.oh_heavy_attack_01, true);
        lastAttack = weapon.oh_heavy_attack_01;
    }
}
