using UnityEngine;
using System.Collections;

public class playercontroller : MonoBehaviour {

	NavMeshAgent agent;
    Vector3 target;
	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();
        
	}

    void MoveTo(Vector3 pos) {
        target = pos;
        agent.SetDestination(target);
    }
	
	// Update is called once per frame
	void Update () {

       

            //Vector3 v3 = Input.mousePosition;
            //v3.z = 20f;
            //v3 = Camera.main.ScreenToWorldPoint(v3);
            //transform.position = v3;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            transform.position = hit.point;
        }
        
	
	}
}
