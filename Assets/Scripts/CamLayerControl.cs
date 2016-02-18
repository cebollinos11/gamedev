using UnityEngine;
using System.Collections;

public class CamLayerControl : MonoBehaviour {


    Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
             // Switch off layer 14, leave others as-is
            cam.cullingMask = ~(1 << 9);
            cam.cullingMask |= (1 << 10);
            

            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Switch on layer 14, leave others as-is
            cam.cullingMask |= (1 << 9);
            cam.cullingMask = ~(1 << 10);
        }
	
	}

}
