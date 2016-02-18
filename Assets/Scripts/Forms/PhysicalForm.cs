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
                    spawnedForms[0] = Instantiate(spawnableForms[0], spawnpoint.position, Quaternion.identity) as Player;
                    spawnedForms[0].state = playerState.idle;
                }
            }
        }
	}
}
