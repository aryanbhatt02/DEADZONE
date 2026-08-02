using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onHold;
    public UnityEvent onRelease;

    public void OnPointerDown(PointerEventData eventData)
    {
        onHold.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onRelease.Invoke();
    }
    
}
