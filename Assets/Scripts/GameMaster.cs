using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    enum turns
    {
        player,
        enemy
    }
    turns curTurn = turns.player;

    //[SerializeField] Enemy[] enemiesOnMap;
    int curEnemy = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    switch(curTurn)
        {
            case turns.player:
                playerTurn();
                break;
            case turns.enemy:
                enemyTurn();
                break;
            default:
                break;
        }
	}

    void playerTurn()
    {
        /*
         * if(player out of actionpoints)
         * {
         * curTurn = turns.enemy;
         * }
         * */
    }

    void enemyTurn()
    {
        /*
         * if(enemiesOnMap[curEnemy] is out of actionpoints)
         * {
         *      if(enemiesOnMap.count+1 >= curEnemy)
         *      {
         *          curEnemy = 0;
         *          curTurn = turns.player;
         *      }
         *      else
         *      {
         *          curEnemy++;
         *      }
         * }
         * */
    }

    void btnEndTurn()
    {
        //if the end turn btn is clicked and its currently the players turn, then change turn to enemy
        if(curTurn == turns.player)
        {
            curTurn = turns.enemy;
        }
    }
}
