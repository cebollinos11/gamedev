using UnityEngine;
using System.Collections;

public class physicalFormController : MonoBehaviour {




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.E))
        {
            //try to explode
            InternetTerminal[] terminals = GameObject.FindObjectsOfType<InternetTerminal>();

            bool isTerminalClose = false;

            for (int i = 0; i < terminals.Length; i++)
            {
                if (Vector3.Distance(transform.position, terminals[i].transform.position) < 4)
                {
                    Debug.Log("can split");
                    isTerminalClose = true;
                }
            }

            if (isTerminalClose)
            {
                Debug.Log("EXPLODEEEEEE");
            }

            else {
                Debug.Log("cannot split now into terminal form");
            }

            
        }


	}
}
