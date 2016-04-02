using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trap : MonoBehaviour {
    GameMaster gamemaster;

    BoxCollider myCollider;
    Bounds myColliderBounds;

    int activeTurns = 1;
    int turnsLeft;

    bool trapActivated = false;

    int turnCheck = 0;

    Enemy[] trappedEnemies;
	// Use this for initialization
	void Start () {
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        GetComponent<MeshRenderer>().enabled = false;
        myCollider = GetComponent<BoxCollider>();
        myColliderBounds = myCollider.bounds;

        turnsLeft = activeTurns;
	}
	
	// Update is called once per frame
	void Update () {
	    if(trapActivated)
        {
            if (turnsLeft <= 0)
            {
                //reenable all enemies in trappedEnemies
                foreach(Enemy enemy in trappedEnemies)
                {
                    enemy.state = Enemy.enemyState.idle;
                }

                turnsLeft = activeTurns;
                trapActivated = false;
            }
            else
            {
                //wait turns
                if(turnCheck != gamemaster.turnNumber)
                {
                    turnsLeft--;
                    turnCheck = gamemaster.turnNumber;
                    Debug.Log("new turn!");
                }
            }
        }
	}

    public void activateMe(int activeTurns)
    {
        if (!trapActivated)
        {
            this.activeTurns = activeTurns;
            turnsLeft = activeTurns;

            turnCheck = gamemaster.turnNumber;

            List<Enemy> enemiesTrapped = new List<Enemy>();

            //check every enemy inside collider
            Enemy[] enemyOnMap = GameObject.FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemyOnMap)
            {
                Vector3 pos = enemy.transform.position;
                if(myColliderBounds.Contains(pos))
                {
                    enemiesTrapped.Add(enemy);
                    enemy.state = Enemy.enemyState.inactive;
                }
            }

            trappedEnemies = enemiesTrapped.ToArray();

            Debug.Log("trapped:" + trappedEnemies.Length);

            //activate trap
            trapActivated = true;
        }
        else
        {
            this.activeTurns = activeTurns;
            turnsLeft = activeTurns;
            turnCheck = gamemaster.turnNumber;
        }
    }
}
