using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    bool haveObject;
    GameObject thingy;
	void Start () {
		
	}
	
	void Update () {
		if(haveObject)
        {
            carry(thingy);
        }
        else
        {
            create();
        }
	}

    void create()
    {
        if(Input.GetKeyDown (KeyCode.E))
    }
}
