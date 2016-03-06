using UnityEngine;
using System.Collections;

public class ElectronicUnit : MonoBehaviour {

    public bool isUsable = true;

    Transform spawnpoint;
	// Use this for initialization
	void Start () {
        spawnpoint = transform.FindChild("spawnpoint");
	}

    public Vector3 getSpawnPoint()
    {
        return spawnpoint.position;
    }
}
