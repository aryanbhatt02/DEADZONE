using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager;

    [SerializeField] Transform weaponShop;

    [Header("player")]
    [SerializeField] Transform player;

    [Header("Shop Button")]
    [SerializeField] float accessDistance = 2f;
    [SerializeField] GameObject weaponShopButton;
    [SerializeField] Image openWeaponShop;
    [SerializeField] TMP_Text weaponShopButtonText;

    [Header("Opened Shop Settings")]
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject gunPriceButton;
    [SerializeField] GameObject weaponShopText;
    [SerializeField] GameObject notEnoughMoneyText;

    [Header("Refill Ammo")]
    [SerializeField] GameObject rifillAmmoButton;
    [SerializeField] int refillAmmoCost = 300;
    [SerializeField] GameObject refilledAmmoText;

    [Header("Unloack Weapons")]
    [SerializeField] int m48Index = 1;    
    [SerializeField] int m48Price = 500;    
    [SerializeField] GameObject m48equipedButton;
    [SerializeField] GameObject purchasedText;
    [SerializeField] GameObject m48equipButton;

    [Header("World Space Image")]
    [SerializeField] Image weaponShopImage;
    [SerializeField] float imageRotateSpeed = 25f;


    private bool openWeapShop;
    private bool purchased;

    void Start()
    {
        weaponShopButton.gameObject.SetActive(false);
        openWeaponShop.gameObject.SetActive(false);
        weaponShopButtonText.gameObject.SetActive(false);
        weaponShopText.gameObject.SetActive(false);
        notEnoughMoneyText.gameObject.SetActive(false);
        refilledAmmoText.SetActive(false);
        gunPriceButton.gameObject.SetActive(false);
        m48equipButton.gameObject.SetActive(false);
        weaponShopImage.gameObject.SetActive(true);
    }


    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, weaponShop.transform.position);

        if (distance < accessDistance)
        {
            weaponShopButton.SetActive(true);
            weaponShopButtonText.gameObject.SetActive(true);
            weaponShopImage.gameObject.SetActive(false);

            if (openWeapShop)
            {
                openWeaponShop.gameObject.SetActive(true);                
                weaponShopButton.SetActive(false);
                weaponShopButtonText.gameObject.SetActive(false);
            }
            else
            {
                openWeaponShop.gameObject.SetActive(false);
            }

        }
        else
        {
            weaponShopButton.SetActive(false);
            weaponShopButtonText.gameObject.SetActive(false);
            openWeaponShop.gameObject.SetActive(false) ;
            weaponShopImage.gameObject.SetActive(true);
            weaponShopImage.transform.Rotate(0f, imageRotateSpeed * Time.deltaTime, 0f, Space.World);
        }
        
    }    

    public void OpenWeaponShop()
    {
        AudioManager.instance.PlayButtonSound();
        openWeapShop = true;
        weaponShopText.gameObject.SetActive(true);
        gunPriceButton.SetActive(true);        
    }

    public void LeaveShop()
    {
        AudioManager.instance.PlayButtonSound();
        openWeapShop = false;
        weaponShopText.gameObject.SetActive(false);
        gunPriceButton.SetActive(false);
    }

    public void RefillAmmo()
    {
        if (weaponManager.GetCurrentWeapon().totalBullets == weaponManager.GetCurrentWeapon().maxBullets)
        {
            return;
        }
        if (!MoneyManager.instance.SpendMoney(refillAmmoCost))
        {
            AudioManager.instance.PlayInvalidSound();

            notEnoughMoneyText.SetActive(true);
            Invoke(nameof(HideNotEnoughMoney), 0.5f);
            return;
        }
        weaponManager.GetCurrentWeapon().RefillAmmo();

        weaponShopText.SetActive(false);
        notEnoughMoneyText.SetActive(false);

        refilledAmmoText.SetActive(true);

        AudioManager.instance.PlayCompletedSound();

        Animator anim = refilledAmmoText.GetComponent<Animator>();
        anim.Play("Refilled Ammo", 0, 0f);

        Invoke(nameof(HideAmmoRefilledText), 2f);


    }
    
    private void HideNotEnoughMoney()
    {
        notEnoughMoneyText.SetActive(false);
        
    }

    private void HideAmmoRefilledText()
    {
        refilledAmmoText.SetActive(false);

        if (openWeapShop)
        {
            weaponShopText.SetActive(true);
        }
    }

    public void UnloackWeapon()
    {
        if (purchased)
        {
            gunPriceButton.SetActive(false);
            return;
        }
        if (!MoneyManager.instance.SpendMoney(m48Price))
        {
            AudioManager.instance.PlayInvalidSound();

            notEnoughMoneyText.SetActive(true);
            Invoke(nameof(HideNotEnoughMoney), 0.5f);
            return;            
        }

        AudioManager.instance.PlayPurchasedSound();

        purchased = true;
        weaponShopText.SetActive(false);        
        m48equipButton.SetActive(true);

        purchasedText.gameObject.SetActive(true);
        Animator anim = purchasedText.GetComponent<Animator>();
        anim.Play("Refilled Ammo", 0, 0f);                        
        Invoke(nameof(HidePurchasedText), 2f);        

    }

    private void HidePurchasedText()
    {
        purchasedText.SetActive(false);

        if (openWeapShop)
        {
            weaponShopText.SetActive(true);
        }
    }

    public void EquipButton()
    {
        AudioManager.instance.PlayButtonSound();
        gunPriceButton.SetActive(false);
        m48equipButton.SetActive(false);
        m48equipedButton.SetActive(true);
        weaponManager.UnloackedWeapon(m48Index);
    }

}


//[SerializeField] WeaponManager weaponManager;

//public int weaponindex;
//public int cost;

//private bool purchased;

//void Start()
//{

//}

//public void BuyWeapon()
//{
//    if (!MoneyManager.instance.SpendMoney(cost)) return;

//    if (purchased) return;

//    weaponManager.UnloackedWeapon(weaponindex);
//    purchased = true;
//}