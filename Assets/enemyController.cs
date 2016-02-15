using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

    NavMeshAgent agent;
    public GameObject target;
	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            agent.SetDestination(target.transform.position);
        }
	}
}
