using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    Animator animator;
    CameraManager cameraManager;
    PlayerStatsManager playerStatsManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerLocomotionManager playerLocomotionManager;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        inputManager = GetComponent<InputManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        animator = GetComponentInChildren<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        interactableUIGameObject = GameObject.FindGameObjectWithTag("Pop-Up");
        itemInteractableGameObject = GameObject.FindGameObjectWithTag("Pop-Up 2");
        interactableUIGameObject.SetActive(false);
        itemInteractableGameObject.SetActive(false);
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        
        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        isUsingLeftHand = animator.GetBool("isUsingLeftHand");
        isInvulnerable = animator.GetBool("isInvulnerable");
        isFiringSpell = animator.GetBool("isFiringSpell");
        animator.SetBool("isInAir", isInAir);
        animator.SetBool("isDead", playerStatsManager.isDead);
        animator.SetBool("isBlocking", isBlocking);

        inputManager.TickInput(delta);
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
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 2f, cameraManager.ignoreLayers))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputManager.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if (interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }

            if (itemInteractableGameObject != null && inputManager.a_Input)
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }

    public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
    {
        playerLocomotionManager.rigidbody.velocity = Vector3.zero; //Evitar o jogador de deslizar
        transform.position = playerStandsHereWhenOpeningChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
    }

    #endregion
}
