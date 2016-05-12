using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    AudioClip CheckpointSound;

    bool hasTriggered;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = false;
	}
	
	

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "PhysicalForm") {
            if(!hasTriggered)
                AudioManager.PlayClip(CheckpointSound);
            SendToSingleton(col.gameObject.transform.position);
            
            hasTriggered = true;
        }
    }

    void SendToSingleton(Vector3 pos)
    {
        Debug.Log("Send to singleton");
        CheckPointManager.init();
        CheckPointManager.SaveCheckPoint(Application.loadedLevel, pos);
    }
}
