using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerManager playerManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    
    public string lastAttack;

    LayerMask backStabLayer = 1 << 16;
    LayerMask riposteLayer = 1 << 17;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        playerInventoryManager = GetComponentInParent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        inputManager = GetComponentInParent<InputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currenStamina <= 0)
            return;

        if (inputManager.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.oh_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.oh_light_attack_02, true);
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currenStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true);
            lastAttack = weapon.th_light_attack_01;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.oh_light_attack_01, true);
            lastAttack = weapon.oh_light_attack_01;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currenStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twoHandFlag)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.oh_heavy_attack_01, true);
            lastAttack = weapon.oh_heavy_attack_01;
        }
    }

    #region Ações De Input
    public void HandleRBAction()
    {
        if (playerInventoryManager.rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }
        else if (playerInventoryManager.rightWeapon.isShockCaster || playerInventoryManager.rightWeapon.isFireCaster || playerInventoryManager.rightWeapon.isWaterCaster)
        {
            PerformRBMagicAction(playerInventoryManager.rightWeapon);
        }
    }

    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(inputManager.twoHandFlag);
        }
        else if (playerInventoryManager.leftWeapon.isMeleeWeapon)
        {
            //Fazer um light attack
        }

    }
    #endregion

    #region Ações De Ataques
    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputManager.comboFlag = true;
            HandleWeaponCombo(playerInventoryManager.rightWeapon);
            inputManager.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.canDoCombo)
                return;

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);    
            HandleLightAttack(playerInventoryManager.rightWeapon);
        }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
            return;
            
        if (weapon.isWaterCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isWaterSpell)
            {
                if (playerStatsManager.currentManaPoints >= playerInventoryManager.currentSpell.manaPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", false, true);
                }
            }
        }
        else if (weapon.isShockCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isShockSpell)
            {
                if (playerStatsManager.currentManaPoints >= playerInventoryManager.currentSpell.manaPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", false, true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
            return;

        if (isTwoHanding)
        {
            //Se estivermos a usar as duas mãos numa arma ativar as animações para a arma direita
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.weapon_art, true);
        }
    }

    private void SuccessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraManager, playerWeaponSlotManager);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }

    #endregion

    #region Ações Defensivas
    private void PerformLBBlockingAction()
    {
        if (playerManager.isInteracting)
            return;

        if (playerManager.isBlocking)
            return;

        playerAnimatorManager.PlayTargetAnimation("Block_Loop", false, true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }
    #endregion

    public void AttemptBackStabOrRiposte()
    {
        if (playerStatsManager.currenStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(inputManager.criticalAttackRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                //VERIFICAR PELO ID DE EQUIPA (Para não poder fazer back stab a amigos ou ti próprio?)
                playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Back Stabbed", true);
                //Dar dano
            }
        }
        else if (Physics.Raycast(inputManager.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
        {
            //VERIFICAR PELO ID DE EQUIPA
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Riposte_Stabbed", true);
            }
        }
    }
}
