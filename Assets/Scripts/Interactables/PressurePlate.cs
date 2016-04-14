using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {
    GameMaster gamemaster;

    [SerializeField] Trap[] trapsToActivate;
    [SerializeField] GameObject[] objectsToEnable;
    [SerializeField] GameObject[] objectsToDisable;

    [SerializeField] Color disabledColor, enabledColor;
    MeshRenderer myRenderer;

    [SerializeField] int activeTurns = 1;
    int turnsLeft;
    int turnCheck;

    bool activated = false;
    bool playerIsOnPlate = false;

    GameObject camFocuser;
	// Use this for initialization
	void Start () {
        camFocuser = Resources.Load("FocusCamera") as GameObject;

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

                foreach(GameObject obj in objectsToDisable)
                {
                    obj.SetActive(true);
                }

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

        if(playerIsOnPlate)
        {
            turnCheck = gamemaster.turnNumber;
            turnsLeft = activeTurns;
            foreach (Trap trap in trapsToActivate)
            {
                trap.activateMe(activeTurns);
            }

            if (!activated)
            {
                bool secondCameraActive = false;

                myRenderer.material.color = enabledColor;

                foreach (GameObject obj in objectsToEnable)
                {

                    obj.SetActive(true);

                    if (!secondCameraActive)
                    {
                        secondCameraActive = true;
                        Instantiate(camFocuser, obj.transform.position, Quaternion.identity);
                    }
                }

                foreach (GameObject obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }

                activated = true;
            }


        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Enemy")
        {
            playerIsOnPlate = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Enemy")
        {
            playerIsOnPlate = false;
        }
    }
}
