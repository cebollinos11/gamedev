using UnityEngine;
using System.Collections;

public class PhysicalForm : Player {

    [SerializeField] Player[] spawnableForms;
    [SerializeField] Player[] spawnedForms;

    [SerializeField] Transform spawnpoint;
	// Use this for initialization
	void Start () {
        base.Start();
	}

    void TryToSpawnDigital() {

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
            spawnedForms[0] = Instantiate(spawnableForms[0], spawnpoint.position, Quaternion.identity) as Player;
            spawnedForms[0].state = playerState.idle;
        }

        else
        {
            Debug.Log("cannot split now into terminal form");
        }
    
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();

        if (base.gamemaster.curTurn == GameMaster.turns.player)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                base.state = playerState.idle;
                foreach (Player form in spawnedForms)
                {
                    if(form != null) form.state = playerState.disabled;
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                base.state = playerState.disabled;
                if(spawnedForms[0] != null)
                {
                    foreach (Player form in spawnedForms)
                    {
                        if (form != null) form.state = playerState.disabled;
                    }

                    spawnedForms[0].state = playerState.idle;
                }
                else
                {
                    TryToSpawnDigital();
                }
            }
        }
	}
}
