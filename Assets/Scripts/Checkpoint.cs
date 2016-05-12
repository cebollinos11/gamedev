using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    AudioClip CheckpointSound;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = false;
	}
	
	

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "PhysicalForm") {
            SendToSingleton(col.gameObject.transform.position);
            AudioManager.PlayClip(CheckpointSound);
        }
    }

    void SendToSingleton(Vector3 pos)
    {
        Debug.Log("Send to singleton");
        CheckPointManager.init();
        CheckPointManager.SaveCheckPoint(Application.loadedLevel, pos);
    }
}
