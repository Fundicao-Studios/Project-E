using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorManager animatorManager;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    InputManager inputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;
    LayerMask backStabLayer = 1 << 16;

    private void Awake()
    {
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputManager = GetComponentInParent<InputManager>();
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

    #region Ações De Input
    public void HandleRBAction()
    {
        if (playerInventory.rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }
        else if (playerInventory.rightWeapon.isShockCaster || playerInventory.rightWeapon.isFireCaster || playerInventory.rightWeapon.isWaterCaster)
        {
            PerformRBMagicAction(playerInventory.rightWeapon);
        }
    }
    #endregion

    #region Ações De Ataques
    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputManager.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            inputManager.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.canDoCombo)
                return;

            animatorManager.anim.SetBool("isUsingRightHand", true);    
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
            return;
            
        if (weapon.isShockCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isShockSpell)
            {
                if (playerStats.currentManaPoints >= playerInventory.currentSpell.manaPointCost)
                {
                    playerInventory.currentSpell.AttemptToCastSpell(animatorManager, playerStats);
                }
                else
                {
                    animatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }
    
    private void SuccessfullyCastSpell()
    {
        playerInventory.currentSpell.SuccessfullyCastSpell(animatorManager, playerStats);
    }

    #endregion

    public void AttemptBackStabOrRiposte()
    {
        RaycastHit hit;

        if (Physics.Raycast(inputManager.criticalAttackRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                //VERIFICAR PELO ID DE EQUIPA (Para não poder fazer back stab a amigos ou ti próprio?)
                playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                animatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Back Stabbed", true);
                //Dar dano
            }
        }
    }
}
