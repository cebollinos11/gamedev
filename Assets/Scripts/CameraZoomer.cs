using UnityEngine;
using System.Collections;

public class CameraZoomer : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
	
	}

    void Zoom(float direction) {
        transform.Translate(Vector3.forward * direction * Time.timeScale * speed,Space.Self);
    }
	
	// Update is called once per frame
	void Update () {

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
	
	}
}
