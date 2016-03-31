using UnityEngine;
using System.Collections;

public class CameraFocuser : MonoBehaviour {


    public GameObject[] forms;
    
    Transform target;
    float speed  = 10;

    [HideInInspector]
    public Transform TargetToFollow;

    CameraOrbit cOrbit;
    float distance = 30f;

	// Use this for initialization

    public void FocusOn(Vector3 where)
    {
        
        target.position = where;
        Vector3 centerino = transform.position + transform.forward * distance ;      

        transform.position += (where - centerino);
        

    }

	void Start () {
        cOrbit = GetComponent<CameraOrbit>();
        GoTo("PhysicalForm");
	}

    void GoTo(string s) {
        
        GameObject form = GameObject.Find(s);
        if (form != null)
        {
            target = form.transform;
            
            FocusOn(target.position);
            cOrbit.referencePointObject = target;
            
        }
    
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GoTo("PhysicalForm");
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GoTo("DigitalForm(Clone)");            
        }

        if (TargetToFollow != null)
        {
            target = TargetToFollow;
            FocusOn(TargetToFollow.position);
            cOrbit.referencePointObject = TargetToFollow;
        }

       
	    
	}
}
