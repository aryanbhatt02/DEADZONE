using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugger : MonoBehaviour
{

    [SerializeField] Vector2 moveInput;

    public bool running;

    [SerializeField] PlayerInputs playerInput;



    private void Awake()
    {
        playerInput = new PlayerInputs();
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Player.Sprint.performed += OnRun;
        playerInput.Player.Sprint.canceled += OnRun;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnRun(InputAction.CallbackContext context)
    {
        running = context.performed;
    }




}

