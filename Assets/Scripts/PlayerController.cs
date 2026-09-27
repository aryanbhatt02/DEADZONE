using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Move")]
    [SerializeField] Vector2 moveInput;
    private bool running;
    [SerializeField] CharacterController controller;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    float moveSpeed;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 2.0f;
    private bool jump;


    public bool IsWalking;
    public bool IsRunning;

    private Vector3 moveDirection;

    [Header("Gravity")]
    private float _gravity = -9.81f;
    [SerializeField] float gravityMultiplier = 3.0f;
    private float _velocity;

    

    void Start()
    {
        //InputManager.Instance.playerInput.Player.Move.performed += HandleMoveInput;
        //InputManager.Instance.playerInput.Player.Move.canceled += HandleMoveInput;
        //InputManager.Instance.playerInput.Player.Sprint.performed += HandleRunningInput;
        //InputManager.Instance.playerInput.Player.Sprint.canceled += HandleRunningInput;

        //InputManager.Instance.playerInput.Player.Jump.performed += HandleJumpInput;


        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
    }

    
    void Update()
    {
        moveInput = InputManager.Instance.moveInput;
        running = InputManager.Instance.Running;

        moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        if(running == true)
        {
            moveSpeed = runSpeed;
            AudioManager.instance.PlayPlayerRunSound();                  
        }
        else
        {
            moveSpeed = walkSpeed;            
            AudioManager.instance.PlayPlayerWalkSound();              
        }

        if(controller.velocity.magnitude > 0.01f)
        {
            IsWalking = moveSpeed == walkSpeed;
            IsRunning = moveSpeed == runSpeed;            
        }
        else
        {
            IsWalking = false;
            IsRunning = false;
            AudioManager.instance.StopFootStepSound();
        }

        

        ApplyGravity();
    }

    //void HandleMoveInput(InputAction.CallbackContext context)
    //{
    //    moveInput = context.ReadValue<Vector2>();
    //}

    //void HandleRunningInput(InputAction.CallbackContext context)
    //{
    //    running = context.performed;
    //}

    //void HandleJumpInput(InputAction.CallbackContext context)
    //{
    //    if (controller.isGrounded)
    //    {
    //        jump = true;
    //    }   
    //}


    private void ApplyGravity()
    {
        if (controller.isGrounded && _velocity < 0.0f)
        {
            _velocity = - 1.0f;
        }
        if (jump)
        {
            _velocity = Mathf.Sqrt(jumpHeight * -2f * _gravity * gravityMultiplier);
            jump = false;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        moveDirection.y = _velocity;
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }




}
