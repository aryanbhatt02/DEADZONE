using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{

    public WeaponController[] weapons;    

    private int currentWeapon;

    void Start()
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        weapons[0].isUnloacked = true;        
        EquipWeapon(0);
    }

    
    void Update()
    {
        //CheckInputs();
    }

    //void CheckInputs()
    //{
    //    if (Keyboard.current.digit1Key.wasPressedThisFrame)
    //    {
    //        EquipWeapon(0);
    //    }
    //    if (Keyboard.current.digit2Key.wasPressedThisFrame)
    //    {
    //        EquipWeapon(1);
    //    }

    //    float scroll = Mouse.current.scroll.magnitude;
    //    if (scroll > 0)
    //    {
    //        NextWeapon();
    //    }
    //    if (scroll < 0)
    //    {
    //        PreviousWeapon();
    //    }
    //}

    public void EquipWeapon(int index)
    {
        if (index >= weapons.Length) return;
        if (!weapons[index].isUnloacked) return;
        if (weapons[currentWeapon].reloding) return;

        for(int i = 0;i < weapons.Length; i++)
        {
            weapons [i].gameObject.SetActive(false);
        }
        weapons[index].gameObject.SetActive(true);
        currentWeapon = index;
    }

    public void NextWeapon()
    {
        int next = currentWeapon + 1;
        if(next >= weapons.Length)
        {
            next = 0;
        }
        TrySwitch(next);
    }

    //private void PreviousWeapon()
    //{
    //    int prev = currentWeapon - 1;
    //    if (prev < weapons.Length)
    //    {
    //        prev = weapons.Length - 1;
    //    }
    //    TrySwitch(prev);
    //}

    private void TrySwitch(int index)
    {
        if (!weapons[index].isUnloacked) return;
        if (weapons[currentWeapon].reloding) return;
        EquipWeapon(index);
    }

    public void UnloackedWeapon(int index)
    {
        if (index >= weapons.Length) return;

        weapons[index].isUnloacked = true;
    }

    public WeaponController GetCurrentWeapon()
    {
        return weapons[currentWeapon];
    }
}
