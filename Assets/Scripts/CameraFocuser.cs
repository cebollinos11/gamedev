using UnityEngine;
using System.Collections;

public class CameraFocuser : MonoBehaviour {


    public GameObject[] forms;
    
    Transform target;
    float speed  = 10;

    [HideInInspector]
    public Transform TargetToFollow;

    CameraOrbit cOrbit;

	// Use this for initialization

    public void FocusOn(Vector3 where)
    {
        Debug.Log(target);
        target.position = where;
        Vector3 centerino = transform.position + transform.forward * 30f ;      

        transform.position += (where - centerino);
        

    }

	void Start () {
        cOrbit = GetComponent<CameraOrbit>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject form = GameObject.Find("PhysicalForm");
            if (form != null)
            {
                target = form.transform;
                FocusOn(target.position);
                cOrbit.referencePointObject = target;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject form = GameObject.Find("DigitalForm(Clone)");
            if(form != null)
            {
                target = form.transform;
                FocusOn(target.position);
                cOrbit.referencePointObject = target;
            }
        }

        if (TargetToFollow != null)
        {
            target = TargetToFollow;
            FocusOn(TargetToFollow.position);
            cOrbit.referencePointObject = TargetToFollow;
        }

       
	    
	}
}
