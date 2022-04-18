using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator anim;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;

    public bool isInteracting;

    [Header("Flags do Jogador")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");

        inputManager.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        cameraManager.FollowTarget(delta);
        cameraManager.HandleCameraRotaion(delta, inputManager.mouseX, inputManager.mouseY);
    }

    private void LateUpdate()
    {
        inputManager.rollFlag = false;
        inputManager.sprintFlag = false;
        inputManager.rb_Input = false;
        inputManager.rt_Input = false;

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
}
