using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {
    GameMaster gamemaster;

    [SerializeField] Trap[] trapsToActivate;
    [SerializeField] GameObject[] objectsToEnable;

    [SerializeField] Color disabledColor, enabledColor;
    MeshRenderer myRenderer;

    [SerializeField] int activeTurns = 1;
    int turnsLeft;
    int turnCheck;

    bool activated = false;
	// Use this for initialization
	void Start () {
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material.color = disabledColor;

        turnsLeft = activeTurns;
	}
	
	// Update is called once per frame
	void Update () { 
	    if(activated)
        {
            if(turnsLeft <= 0)
            {
                foreach (GameObject obj in objectsToEnable)
                {
                    obj.SetActive(false);
                }

                turnsLeft = activeTurns;
                myRenderer.material.color = disabledColor;
                activated = false;
            }
            else
            {
                if (turnCheck != gamemaster.turnNumber)
                {
                    turnsLeft--;
                    turnCheck = gamemaster.turnNumber;
                    Debug.Log("new turn!");
                }
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(!activated)
        {
            if(other.transform.tag == "Player")
            {
                turnCheck = gamemaster.turnNumber;
                myRenderer.material.color = enabledColor;

                foreach (Trap trap in trapsToActivate)
                {
                    trap.activateMe(activeTurns);
                }

                foreach(GameObject obj in objectsToEnable)
                {
                    obj.SetActive(true);
                }

                activated = true;
            }
        }
    }
}
