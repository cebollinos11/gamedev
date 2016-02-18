using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public enum turns
    {
        player,
        enemy
    }
    public turns curTurn = turns.player;

    [SerializeField] Enemy[] enemiesOnMap;
    public int curEnemy = 0;

    Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            btnEndTurn();
        }
	}

    void playerTurn()
    {
        
    }

    void enemyTurn()
    {
        if (enemiesOnMap.Length > 0)
        {
            if (enemiesOnMap[curEnemy].state == Enemy.enemyState.idle)
            {
                if (enemiesOnMap.Length - 1 <= curEnemy)
                {
                    curEnemy = 0;
                    curTurn = turns.player;
                    player.resetTurn();
                }
                else
                {
                    curEnemy++;
                    enemiesOnMap[curEnemy].state = Enemy.enemyState.playTurn;
                }
            }
        }
        else
        {
            curTurn = turns.player;
            player.resetTurn();
        }
    }

    void btnEndTurn()
    {
        //if the end turn btn is clicked and its currently the players turn, then change turn to enemy
        if (player.state == Player.playerState.idle && curTurn == turns.player)
        {
            if (enemiesOnMap.Length > 0) 
            { 
                enemiesOnMap[curEnemy].state = Enemy.enemyState.playTurn; 
            }

            curTurn = turns.enemy;
        }
    }
}
