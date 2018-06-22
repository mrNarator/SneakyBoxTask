﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour {

    [SerializeField]
    float rotationSensitivity;
    [SerializeField]
    float dragSensitivity;
    [SerializeField]
    float zoomSpeed = 5;
    [SerializeField]
    float smooth = 1;
    [SerializeField]
    GameObject thingy;

    bool rightclicked;
    bool leftclicked;

    Vector3 dragOrigin;

    private Vector3 ResetCameraP;
    private Quaternion ResetCameraR;
    private Vector3 Origin;
    private Vector3 oldPos;
    private bool Drag;



    bool haveObject;
    GameObject carriedObject;
    public float rayLength;
    public LayerMask layermask;
    public float carriedObjectRotationSpeed = 10f;
    public float objectOffsetY = 7f;
    public float objectOffsetX = 7f;
    public float objectOffsetZ = 7f;

    public GameObject addedObjects;
    void Start()
    {
        ResetCameraP = transform.position;
        ResetCameraR = transform.rotation;

        addedObjects = new GameObject("addedObjects");

    }
    void Update()
    {
        if (haveObject)
        {
            carry(carriedObject);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(carriedObject);
                haveObject = false;
            }
        }
        else
        {
            carriedObject = create();
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

        if (Input.GetMouseButton(1))
        {
            var mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.Rotate(mousePos.y * rotationSensitivity, 0, mousePos.x * rotationSensitivity);
        }

        if (!haveObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Drag = true;
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
        }

        //reset camera with wheel clicl
        if (Input.GetMouseButton(2))
        {
            transform.position = ResetCameraP;
            transform.rotation = ResetCameraR;
        }
    }


    GameObject create()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject carriedThingy = Instantiate(thingy, 
                new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    Input.mousePosition.z
                    ), transform.rotation, addedObjects.transform);
            haveObject = true;
            return carriedThingy;
        }
        return null;
    }

    void carry(GameObject o)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, layermask))
        {
            float objectSizeX = o.GetComponent<Renderer>().bounds.extents.x;
            float objectSizeY = o.GetComponent<Renderer>().bounds.extents.y;
            float objectSizeZ = o.GetComponent<Renderer>().bounds.extents.z;

            //rester rotation
            o.transform.rotation = new Quaternion();
            //new position on wall/floor
            o.transform.position = new Vector3(
                hit.transform.position.x + hit.normal.x * objectSizeX, 
                hit.transform.position.y + hit.normal.y * objectSizeY, 
                hit.transform.position.z + hit.normal.z * objectSizeZ
                );

            if(Input.GetMouseButtonDown(0))
            {
                
                GameObject carriedThingy = Instantiate(thingy, 
                    new Vector3 (
                        hit.transform.position.x + hit.normal.x * objectSizeX, 
                        hit.transform.position.y + hit.normal.y * objectSizeY,
                        hit.transform.position.z + hit.normal.z * objectSizeZ
                    ), o.transform.rotation, addedObjects.transform);

                carriedThingy.tag = "temp";
                Destroy(o);
                haveObject = false;
            }
        }
        //if cursor is not on the room
        else
        {
            o.transform.position = transform.position + transform.forward * objectOffsetY + transform.right * objectOffsetX + transform.up * objectOffsetZ;
            o.transform.Rotate(new Vector3(Time.deltaTime*carriedObjectRotationSpeed, 0, 0));

        }
    }
}