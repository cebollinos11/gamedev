using UnityEngine;
using System.Collections;

public class FocusCamera : MonoBehaviour {

    public float TimeToLive;

	// Use this for initialization
	void Start () {
        Invoke("DestroySelf", TimeToLive);
	}

    void DestroySelf() {
        Destroy(gameObject);
    }

    void Update() {
        transform.Rotate(new Vector3(0, 1f, 0) * Time.deltaTime);
    }
	
	
}
