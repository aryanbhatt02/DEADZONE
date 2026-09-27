using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{

    [SerializeField] Transform playerBody;
    [SerializeField] float yClamp;
    [SerializeField] float senstivity;

    float xRotation;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector2 mouse = DragArea.swipeDelta;

        float mouseX = mouse.x * senstivity * Time.deltaTime;
        float mouseY = mouse.y * senstivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -yClamp, yClamp);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

        DragArea.swipeDelta = Vector2.zero;
    }
}
