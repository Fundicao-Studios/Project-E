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
    
    [Header("Animações De Ataque")]
    string oh_light_attack_01 = "OH_Light_Attack_01";
    string oh_light_attack_02 = "OH_Light_Attack_02";
    string oh_heavy_attack_01 = "OH_Heavy_Attack_01";

    string th_light_attack_01 = "TH_Light_Attack_01";
    string th_light_attack_02 = "TH_Light_Attack_02";

    string weapon_art = "Parry";

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

    public void HandleRBAction()
    {
        if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword 
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformRBMeleeAction();
        }
        else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.ShockCaster 
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.FireCaster 
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.WaterCaster)
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
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield 
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLTWeaponArt(inputManager.twoHandFlag);
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
        {
            //Fazer um light attack
        }

    }



    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        if (inputManager.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == oh_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
            }
            else if (lastAttack == th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
            lastAttack = th_light_attack_01;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
            lastAttack = oh_light_attack_01;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twoHandFlag)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
            lastAttack = oh_heavy_attack_01;
        }
    }



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

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
            return;
            
        if (weapon.weaponType == WeaponType.WaterCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.weaponType == WeaponType.WaterCaster)
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
        else if (weapon.weaponType == WeaponType.ShockCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.weaponType == WeaponType.ShockCaster)
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
            playerAnimatorManager.PlayTargetAnimation(weapon_art, true);
        }
    }



    private void SuccessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraManager, playerWeaponSlotManager);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }

    public void AttemptBackStabOrRiposte()
    {
        if (playerStatsManager.currentStamina <= 0)
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
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
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
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposte_Stabbed", true);
            }
        }
    }
}
