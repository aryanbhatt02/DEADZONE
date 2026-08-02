using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    public static KillCount instance;

    public int currentKillCount = 0;

    private void Awake()
    {
        instance = this;
    }
    
    public void PlusKillCount(int count)
    {
        currentKillCount += count;
        UIManager.Instance.AddKillCount(count);
    }

   
}
