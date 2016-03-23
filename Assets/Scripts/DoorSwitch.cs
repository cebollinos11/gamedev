using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    GameMaster gamemaster;
    FormsManager formManager;
    GameObject[] playerForms;
    bool splitted = false;

    public Transform door;
    [SerializeField] bool complete, physical, digital = false;
    

	// Use this for initialization
	void Start () {
        playerForms = GameObject.FindGameObjectsWithTag("Player");
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        formManager = gamemaster.GetComponent<FormsManager>();

        if(complete)
        {
            physical = digital = complete;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(gamemaster.curTurn == GameMaster.turns.player)
        {
            if(formManager.isSplitted != splitted)
            {
                splitted = formManager.isSplitted;
                playerForms = GameObject.FindGameObjectsWithTag("Player");
            }

            if (complete)
            {

            }
            if(physical)
            {

            }
            if(digital)
            {

            }
        }
	}
}
