using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    Animator animator;
    CameraManager cameraManager;
    PlayerStatsManager playerStatsManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerLocomotionManager playerLocomotionManager;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject interactableUIGameObjectMessage;
    public GameObject itemInteractableGameObject;
    public  PlayerInventoryManager playerInventoryManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public UIManager uiManager;
    public LayerMask whatIsInteractable;
    float messageTimer = 0;
    bool startTimerMessage = false;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        inputManager = GetComponent<InputManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        animator = GetComponentInChildren<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponentInChildren<PlayerEffectsManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
        playerEquipmentManager = GetComponentInChildren<PlayerEquipmentManager>();
        interactableUIGameObject = GameObject.FindGameObjectWithTag("Pop-Up");
        interactableUIGameObjectMessage = GameObject.FindGameObjectWithTag("Pop-Up-Message");
        itemInteractableGameObject = GameObject.FindGameObjectWithTag("Pop-Up 2");
        interactableUIGameObject.SetActive(false);
        interactableUIGameObjectMessage.SetActive(false);
        itemInteractableGameObject.SetActive(false);
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        float delta = Time.deltaTime; 
        
        if (startTimerMessage)
        {
            messageTimer += Time.deltaTime;
        }
        
        if (messageTimer >= 8f)
        {
            startTimerMessage = false;
            messageTimer = 0f;
            itemInteractableGameObject.SetActive(false);
        }

        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        isUsingLeftHand = animator.GetBool("isUsingLeftHand");
        isInvulnerable = animator.GetBool("isInvulnerable");
        isFiringSpell = animator.GetBool("isFiringSpell");
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        animator.SetBool("isInAir", isInAir);
        animator.SetBool("isDead", playerStatsManager.isDead);
        animator.SetBool("isBlocking", isBlocking);

        inputManager.TickInput(delta);
        playerLocomotionManager.rigidbody.AddForce(Vector3.up * 10);
        playerAnimatorManager.canRotate = animator.GetBool("canRotate");
        playerLocomotionManager.HandleRollingAndSprinting(delta);
        playerLocomotionManager.HandleJumping();
        playerStatsManager.RegenerateStamina();

        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraManager != null)
        {
            cameraManager.FollowTarget(delta);
            cameraManager.HandleCameraRotaion(delta, inputManager.mouseX, inputManager.mouseY);
        }

        playerLocomotionManager.HandleFalling(delta, playerLocomotionManager.moveDirection);
        playerLocomotionManager.HandleMovement(delta);
        playerLocomotionManager.HandleDance();
        playerLocomotionManager.HandleRotation(delta);
        playerEffectsManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate()
    {
        inputManager.rollFlag = false;
        inputManager.rb_Input = false;
        inputManager.rt_Input = false;
        inputManager.lt_Input = false;
        inputManager.d_Pad_Up = false;
        inputManager.d_Pad_Down = false;
        inputManager.d_Pad_Left = false;
        inputManager.d_Pad_Right = false;
        inputManager.a_Input = false;
        inputManager.jump_Input = false;
        inputManager.x_Input = false;
        inputManager.inventory_Input = false;

        if (isInAir)
        {
            playerLocomotionManager.inAirTimer = playerLocomotionManager.inAirTimer + Time.deltaTime;
        }
    }

    #region Interações Do Jogador

    public void CheckForInteractableObject()
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, 1.5f, whatIsInteractable);

        for (int i = 0; i < interactables.Length; i++)
        {
            Interactable interactableObject = interactables[i].GetComponent<Interactable>();

            if (interactableObject != null)
            {
                string interactableText = interactableObject.interactableText;
                interactableUI.interactableText.text = interactableText;
                interactableUIGameObject.SetActive(true);

                if (inputManager.a_Input)
                {
                    interactables[i].GetComponent<Interactable>().Interact(this);
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null)
                {
                    if (inputManager.a_Input)
                    {
                        startTimerMessage = true;
                    }
                }
            }
        }
    }

    public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
    {
        interactableUIGameObject.SetActive(false);
        playerLocomotionManager.rigidbody.velocity = Vector3.zero; //Evitar o jogador de deslizar
        transform.position = playerStandsHereWhenOpeningChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
    }

    #endregion
}
