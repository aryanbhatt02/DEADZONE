using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragArea : MonoBehaviour, IDragHandler
{
    public float senstivity = 0.3f;
    public static Vector2 swipeDelta;

    public void OnDrag(PointerEventData eventData)
    {
        swipeDelta = eventData.delta * senstivity;
    }
}
