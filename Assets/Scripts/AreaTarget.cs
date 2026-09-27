using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTarget : MonoBehaviour
{
    [SerializeField] CampArea assignedArea;

    public void DamageArea(float damage)
    {
        assignedArea.DamageArea(damage);
    }


}
