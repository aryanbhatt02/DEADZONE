using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateHealthUI : MonoBehaviour
{

    [SerializeField] Image healthBar;
    [SerializeField] CampArea area;


    void Start()
    {

    }


    void Update()
    {
        healthBar.fillAmount = (area.currentHealth / area.maxHealth);

    }

}