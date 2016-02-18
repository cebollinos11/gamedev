using UnityEngine;
using System.Collections;

public class CameraFocuser : MonoBehaviour {


    public GameObject[] forms;
    
    Vector3 target;
    float speed  = 10;

	// Use this for initialization

    public void FocusOn(Vector3 where)
    {
        target = where;

        Vector3 centerino = transform.position + transform.forward * 60f ;

       

        transform.position += (where - centerino);
        

    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FocusOn(forms[0].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FocusOn(forms[1].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FocusOn(forms[2].transform.position);
        }
	    
	}
}
