using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour {
    
    [SerializeField]
    float rotationSensitivity;
    [SerializeField]
    float dragSensitivity;
    [SerializeField]
    float zoomSpeed= 5;

    bool rightclicked;
    bool leftclicked;

    Vector3 dragOrigin;

    private Vector3 ResetCameraP;
    private Quaternion ResetCameraR;
    private Vector3 Origin;
    private Vector3 oldPos;
    private bool Drag;

    void Start()
    {

        ResetCameraP = transform.position;
        ResetCameraR = transform.rotation;
    }
    void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

        if (Input.GetMouseButton(1))
        {
            var mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.Rotate(mousePos.y * rotationSensitivity, 0, mousePos.x * rotationSensitivity);
        }
        

            if (Input.GetMouseButtonDown(0))
            {
                Drag= true;
                oldPos = transform.position;
                Origin = Camera.main.ScreenToViewportPoint(Input.mousePosition); 
            }

            if (Input.GetMouseButtonDown(0))
            {
                Drag = true;
                oldPos = transform.position;
                Origin = Camera.main.ScreenToViewportPoint(Input.mousePosition);  
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Origin;    
                transform.position = oldPos + -pos * dragSensitivity;     
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drag = false;
            }

        //reset camera with wheel clicl
        if (Input.GetMouseButton(2))
        {
            transform.position = ResetCameraP;
            transform.rotation = ResetCameraR;
        }
    }
}