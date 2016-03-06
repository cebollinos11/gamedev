using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

    public float turnSpeed = 4.0f;
    public Transform referencePointObject;

    Transform refPoint;

    private Vector3 offset;

    private Quaternion originalRotation;
    private Vector3 orignalPosition;

    Vector3 originalAngles;



	// Use this for initialization
	void Start () {
        orignalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        originalAngles = transform.rotation.eulerAngles;

        refPoint = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {

            if (refPoint == null)
                refPoint = referencePointObject;

            //Debug.Log(refPoint.position);
            //transform.LookAt(refPoint);
            if(refPoint != null) transform.RotateAround(refPoint.position, Vector3.up, Input.GetAxis("Mouse X") * turnSpeed);
            //transform.RotateAround(refPoint.position, Vector3.right, Input.GetAxis("Mouse Y") * turnSpeed);

            transform.rotation = Quaternion.Euler(new Vector3(originalAngles.x, transform.rotation.eulerAngles.y, originalAngles.z));

        }
        else {

            refPoint = null;
            //transform.localPosition = Vector3.Lerp(transform.localPosition, orignalPosition, Time.deltaTime);
            //transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, Time.deltaTime); ;
        }
        

    }


}
