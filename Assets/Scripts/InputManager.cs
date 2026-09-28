using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] Image runOnImage;

    public PlayerInputs playerInput;
    public WeaponManager weaponManager;

    public Joystick joystick;
    public Vector2 moveInput;            

    public bool Running = false;
    private bool mobileRunning = false;
    public bool firing = false;
    private bool mobileFiring = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        playerInput = new PlayerInputs();
        playerInput.Enable();
    }

    void Start()
    {
        runOnImage.gameObject.SetActive(false);
    }

    
    void Update()
    {
        Vector2 keyboardInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        if(keyboardInput.magnitude > 0.01f)
        {
            moveInput = keyboardInput;
        }
        else
        {
            moveInput = joystickInput;
        }

        if (playerInput.Player.Sprint.IsPressed())
        {
            Running = true;
        }
        else
        {
            Running = mobileRunning;
        }

        firing = mobileFiring || playerInput.Player.Attack.IsPressed();
    }

    //public void OnDisable()
    //{
    //    playerInput.Disable();
    //}

    public void ToggleRun()
    {
        if (!Running)
        {
            runOnImage.gameObject.SetActive(true);
        }
        else
        {
            runOnImage.gameObject.SetActive(false);
        }
        mobileRunning = !mobileRunning;
        Running = mobileRunning;
    }

    public void HandleFire(bool value)
    {
        mobileFiring = value;
    }

    public void Reload()
    {
        weaponManager.GetCurrentWeapon().WeaponReload();
    }

    public void SwitchWeapon()
    {
        weaponManager.NextWeapon();
    }
}
