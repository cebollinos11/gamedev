using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = false;
	}
	
	

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "PhysicalForm") {
            SendToSingleton(col.gameObject.transform.position); 
        }
    }

    void SendToSingleton(Vector3 pos)
    {
        Debug.Log("Send to singleton");
        CheckPointManager.init();
        CheckPointManager.SaveCheckPoint(Application.loadedLevel, pos);
    }
}
