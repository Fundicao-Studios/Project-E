using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputManager inputManager;
    Animator anim;
    CameraManager cameraManager;
    PlayerStats playerStats;
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
    public bool isInvulnerable;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Start()
    {
        inputManager = GetComponent<InputManager>();
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
        anim.SetBool("isInAir", isInAir);
        isInvulnerable = anim.GetBool("isInvulnerable");

        inputManager.TickInput(delta);
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
    }

    private void LateUpdate()
    {
        inputManager.rollFlag = false;
        inputManager.rb_Input = false;
        inputManager.rt_Input = false;
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

    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraManager.ignoreLayers))
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
}
