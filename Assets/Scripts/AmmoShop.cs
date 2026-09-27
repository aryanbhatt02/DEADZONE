//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AmmoShop : MonoBehaviour
//{

//    [SerializeField] int ammoAmount = 30;
//    [SerializeField] int ammoCost = 100;
//    [SerializeField] int index = 0;
//    [SerializeField] WeaponManager weaponManager;
 
//    void Start()
//    {
        
//    }

//    public void BuyAmmo()
//    {
//        if (!MoneyManager.instance.SpendMoney(ammoCost)) return;

//        weaponManager.weapons[index].totalBullets += ammoAmount;
//    }
   
//}
