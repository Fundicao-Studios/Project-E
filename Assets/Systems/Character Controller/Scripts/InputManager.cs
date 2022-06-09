using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool a_Input;
    public bool x_Input;
    public bool y_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool lb_Input;
    public bool lt_Input;
    public bool critical_Attack_Input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool rollFlag;
    public bool twoHandFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    public Transform criticalAttackRayCastStartPoint;
    public ConsumableItem emptyBottle;
    ConsumableInventorySlot consumableInventorySlot;

    PlayerControls inputActions;
    PlayerCombatManager playerCombatManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager player;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerStatsManager playerStatsManager;
    BlockingCollider blockingCollider;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    ConsumableSlotManager consumableSlotManager;
    CameraManager cameraManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerCombatManager = GetComponentInChildren<PlayerCombatManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        player = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponentInChildren<PlayerEffectsManager>();
        consumableInventorySlot = GetComponentInChildren<ConsumableInventorySlot>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerWeaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
        consumableSlotManager = GetComponentInChildren<ConsumableSlotManager>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;
            inputActions.PlayerActions.LB.performed += i => lb_Input = true;
            inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
            inputActions.PlayerActions.LT.performed += i => lt_Input = true;
            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            inputActions.PlayerQuickSlots.DPadUp.performed += i => d_Pad_Up = true;
            inputActions.PlayerActions.A.performed += i => a_Input = true;
            inputActions.PlayerActions.X.performed += i => x_Input = true;
            inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
            inputActions.PlayerActions.Y.performed += i => y_Input = true;
            inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        if (playerStatsManager.isDead)
            return;

        player.uiManager.UpdateUI();

        HandleMoveInput(delta);
        HandleRollInput(delta);
        HandleCombatInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
        HandleUseConsumableInput();
    }

    private void HandleMoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        if (b_Input)
        {
            rollInputTimer += delta;

            if (playerStatsManager.currentStamina <= 0)
            {
                b_Input = false;
                sprintFlag = false;
            }

            if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;

            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleCombatInput(float delta)
    {
        if (rb_Input)
        {
            playerCombatManager.HandleRBAction();
        }

        if (rt_Input)
        {
            playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
        }

        if (lt_Input)
        {
            if (twoHandFlag)
            {
                //se estiver a usar o modelo da arma com duas mãos
            }
            else
            {
                playerCombatManager.HandleLTAction();
            }
        }

        if (lb_Input)
        {
            playerCombatManager.HandleLBAction();
        }
        else
        {
            player.isBlocking = false;

            if (blockingCollider.blockingCollider.enabled)
            {
                blockingCollider.DisableBlockingCollider();
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_Pad_Right)
        {
            playerInventoryManager.ChangeRightWeapon();
        }
        else if (d_Pad_Left)
        {
            playerInventoryManager.ChangeLeftWeapon();
        }
        else if (d_Pad_Up)
        {
            playerInventoryManager.ChangeConsumable();
        }
    }

    private void HandleInventoryInput()
    {
        if (inventoryFlag)
        {
            player.uiManager.UpdateUI();
        }

        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                Cursor.visible = true;
                player.uiManager.OpenInventories();
                player.uiManager.hudWindow.SetActive(false);
            }
            else
            {
                Cursor.visible = false;
                player.uiManager.OpenInventories();
                player.uiManager.CloseAllInventoryWindows();
                player.uiManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            cameraManager.HandleLockOn();
            if (cameraManager.nearestLockOnTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            cameraManager.ClearLockOnTargets();
        }

        if (lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Left_Input = false;
            cameraManager.HandleLockOn();
            if (cameraManager.leftLockTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.leftLockTarget;
            }
        }

        if (lockOnFlag && right_Stick_Right_Input)
        {
            right_Stick_Right_Input = false;
            cameraManager.HandleLockOn();
            if (cameraManager.rightLockTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.rightLockTarget;
            }
        }

        cameraManager.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
        if (y_Input)
        {
            y_Input = false;

            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                player.isTwoHandingWeapon = true;
            }
            else
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                player.isTwoHandingWeapon = false;
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (critical_Attack_Input)
        {
            critical_Attack_Input = false;
            playerCombatManager.AttemptBackStabOrRiposte();
        }
    }

    private void HandleUseConsumableInput()
    {
        if (x_Input)
        {
            if (playerInventoryManager.currentConsumable == null)
                return;
                
            x_Input = false;
            playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, consumableSlotManager, playerEffectsManager);
        }
    }
}
