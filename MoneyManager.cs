using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int currentMoney = 200;

    void Awake()
    {
        instance = this;
    }

   public void AddMoney(int amount)
    {
        currentMoney += amount;
        UIManager.Instance.AddMoney(amount);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney < amount) return false;

        currentMoney -= amount;
        UIManager.Instance.RemoveMoney(amount);
        return true;
    }

    public bool HasEnoughMoney(int amount)
    {
        return currentMoney >= amount;
    }

}
