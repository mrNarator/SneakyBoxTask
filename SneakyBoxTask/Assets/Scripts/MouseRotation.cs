using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour {
    //materialas
    [SerializeField]
    Material woodMaterial;
    [SerializeField]
    Material metalMaterial;
    [SerializeField]
    Material swagMaterial;
    [SerializeField]
    Material defaultMaterial;

    //offsets constants
    [SerializeField]
    public float objectOffsetY = 7f;
    [SerializeField]
    public float objectOffsetX = 7f;
    [SerializeField]
    public float objectOffsetZ = 7f;
    [SerializeField]
    public float matOffsetX = 7f;
    [SerializeField]
    public float matOffsetY = 7f;
    [SerializeField]
    public float matOffsetZ = 7f;

    //camera manipulation
    [SerializeField]
    float rotationSensitivity;
    [SerializeField]
    float dragSensitivity;
    [SerializeField]
    float zoomSpeed = 5;
    [SerializeField]
    float smooth = 1;

    // raycast and creation
    [SerializeField]
    float rayLength;
    [SerializeField]
    LayerMask layermaskWall;
    [SerializeField]
    LayerMask layermaskItem;
    [SerializeField]
    float carriedObjectRotationSpeed = 10f;
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

    public LinkedList<GameObject> selected;

    bool haveObject;
    bool materialChange;
    GameObject carriedObject;
    GameObject addedObjects;

    GameObject mats;
    GameObject mat1;
    GameObject mat2;
    GameObject mat3;
    void Start()
    {
        selected = new LinkedList<GameObject>();

        // for camera reset
        ResetCameraP = transform.position;
        ResetCameraR = transform.rotation;

        // container for new objects
        addedObjects = new GameObject("addedObjects");

        // create material selections
        mats = new GameObject("mats");
        mat1 = Instantiate(thingy, new Vector3(), transform.rotation, mats.transform);
        mat2 = Instantiate(thingy, new Vector3(), transform.rotation, mats.transform);
        mat3 = Instantiate(thingy, new Vector3(), transform.rotation, mats.transform);

        mat1.GetComponent<Renderer>().material = woodMaterial;
        mat2.GetComponent<Renderer>().material = metalMaterial;
        mat3.GetComponent<Renderer>().material = swagMaterial;

        mats.SetActive(false);

    }
    void Update()
    {
        // if you have object place it, if you dont create it
        if (haveObject)
        {
            carry(carriedObject);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(carriedObject);
                haveObject = false;
            }
        }
        else if (materialChange)
        {
            //select for material change
            select();
            renderMaterialsSelection();
            if (Input.GetKeyDown(KeyCode.W))
            {
                materialChange = false;
                mats.SetActive(false);
                removeSelect();
            }
        }
        else
        {
            //create Item to place

            if (Input.GetKeyDown(KeyCode.E))
            {
                carriedObject = create();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                materialChange= true;
                mats.SetActive(true);
            }
        }

        // zoom in zoom out camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

        // rotate camera
        if (Input.GetMouseButton(1))
        {
            var mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.Rotate(mousePos.y * rotationSensitivity, 0, mousePos.x * rotationSensitivity);
        }

        // if you are handling object, disable camera drag
        if (!haveObject&&!materialChange)
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


    // creates object that can be placed on walls or floor
    GameObject create()
    {
        removeSelect();
        GameObject carriedThingy = Instantiate(thingy, 
            new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                Input.mousePosition.z
                ), transform.rotation, addedObjects.transform);
        haveObject = true;
        return carriedThingy;
    }

    void select()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, layermaskItem))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Renderer rend = hit.transform.gameObject.GetComponent<Renderer>();
                rend.material.SetColor("_Color", Color.green);
                selected.AddFirst(hit.transform.gameObject);
            }
        }
    }

    //remove selected list of items for material change
    void removeSelect()
    {
        while (selected.Count != 0)
        {
            GameObject temp = selected.First.Value;
            temp.transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            selected.RemoveFirst();
        }
    }
    void renderMaterialsSelection()
    {
        mat1.transform.position = transform.position + transform.forward * matOffsetY + transform.right * matOffsetX + transform.up * matOffsetZ;
        mat2.transform.position = transform.position + transform.forward * matOffsetY + transform.right * (matOffsetX+4) + transform.up * matOffsetZ;
        mat3.transform.position = transform.position + transform.forward * matOffsetY + transform.right * (matOffsetX+8) + transform.up * matOffsetZ;

        mat1.transform.Rotate(new Vector3(Time.deltaTime * carriedObjectRotationSpeed, 0, 0));
        mat2.transform.Rotate(new Vector3(Time.deltaTime * carriedObjectRotationSpeed, 0, 0));
        mat3.transform.Rotate(new Vector3(Time.deltaTime * carriedObjectRotationSpeed, 0, 0));
    } 
    // controls object carrying and placement
    void carry(GameObject o)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, layermaskWall))
        {
            // object size constants
            float objectSizeX = o.GetComponent<Renderer>().bounds.extents.x;
            float objectSizeY = o.GetComponent<Renderer>().bounds.extents.y;
            float objectSizeZ = o.GetComponent<Renderer>().bounds.extents.z;

            //reset rotation
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
                carriedThingy.layer = LayerMask.NameToLayer("items");
                carriedThingy.GetComponent<Renderer>().material = defaultMaterial;
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