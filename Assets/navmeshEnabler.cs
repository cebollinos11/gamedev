using UnityEngine;
using System.Collections;

public class navmeshEnabler : MonoBehaviour {

    NavMeshAgent nma;

	// Use this for initialization
    void EnableNavMesh()
    {
        nma.enabled = true;
    }
    
    void Awake () {
        nma = GetComponent<NavMeshAgent>();
        nma.enabled = false;
        Invoke("EnableNavMesh", 1f);
	}
	
	
}
