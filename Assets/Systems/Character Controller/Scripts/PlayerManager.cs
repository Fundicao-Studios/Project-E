using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    Animator anim;
    CameraManager cameraManager;
    PlayerStats playerStats;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerLocomotion playerLocomotion;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    public bool isInteracting;

    [Header("Flags do Jogador")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        inputManager = GetComponent<InputManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        anim = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        interactableUI = FindObjectOfType<InteractableUI>();
        interactableUIGameObject = GameObject.FindGameObjectWithTag("Pop-Up");
        itemInteractableGameObject = GameObject.FindGameObjectWithTag("Pop-Up 2");
        interactableUIGameObject.SetActive(false);
        itemInteractableGameObject.SetActive(false);
    }

    void Update()
    {
        float delta = Time.deltaTime;
        
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isUsingRightHand = anim.GetBool("isUsingRightHand");
        isUsingLeftHand = anim.GetBool("isUsingLeftHand");
        isInvulnerable = anim.GetBool("isInvulnerable");
        isFiringSpell = anim.GetBool("isFiringSpell");
        anim.SetBool("isBlocking", isBlocking);
        anim.SetBool("isInAir", isInAir);
        anim.SetBool("isDead", playerStats.isDead);

        inputManager.TickInput(delta);
        playerAnimatorManager.canRotate = anim.GetBool("canRotate");
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleJumping();
        playerStats.RegenerateStamina();

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

        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleDance();
        playerLocomotion.HandleRotation(delta);
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
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
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
        playerLocomotion.rigidbody.velocity = Vector3.zero; //Evitar o jogador de deslizar
        transform.position = playerStandsHereWhenOpeningChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
    }

    #endregion
}
