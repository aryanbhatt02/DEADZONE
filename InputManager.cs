using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public PlayerInputs playerInput;

    public Joystick joystick;
    public Vector2 moveInput;
    public bool Running = false;

    public bool firing = false;
    public WeaponManager weaponManager;

    [SerializeField] Image runOnImage;

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

        //playerInput = new PlayerInputs();
        //playerInput.Enable();
    }

    void Start()
    {
        runOnImage.gameObject.SetActive(false);
    }

    
    void Update()
    {
        moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
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
        Running = !Running;
    }

    public void HandleFire(bool value)
    {
        firing = value;
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
