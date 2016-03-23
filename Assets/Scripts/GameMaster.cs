using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameMaster : MonoBehaviour {

    //UI elements
    Transform gameUI;
    Text txtCurTurn;
    Button btnEndTurn;
    //
    public enum turns
    {
        player,
        enemy
    }
    public turns curTurn = turns.player;

    [SerializeField] Enemy[] enemiesOnMap;
    public int curEnemy = 0;

    FormsManager formManager;

	// Use this for initialization
	void Start () {
        formManager = GetComponent<FormsManager>();

        gameUI = GameObject.FindGameObjectWithTag("gameUI").transform;
        txtCurTurn = gameUI.FindChild("txtTurn").GetComponent<Text>();
        btnEndTurn = gameUI.FindChild("btnEndTurn").GetComponent<Button>();
        btnEndTurn.onClick.AddListener(() => endTurnBtnClicked());

        //grab all enemies from map
        enemiesOnMap = GameObject.FindObjectsOfType<Enemy>();
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
            endTurnBtnClicked();
        }
	}

    void playerTurn()
    {
        if(txtCurTurn.text != "Turn: Player")
        {
            txtCurTurn.text = "Turn: Player";
        }
    }

    void enemyTurn()
    {
        if(txtCurTurn.text != "Turn: Enemy")
        {
            txtCurTurn.text = "Turn: Enemy";
        }

        if (enemiesOnMap.Length > 0)
        {
            if (enemiesOnMap[curEnemy].state == Enemy.enemyState.idle)
            {
                if (enemiesOnMap.Length - 1 <= curEnemy)
                {
                    curEnemy = 0;
                    curTurn = turns.player;

                    formManager.resetTurn();
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
            formManager.resetTurn();
        }
    }

    void endTurnBtnClicked()
    {
        //if the end turn btn is clicked and its currently the players turn, then change turn to enemy
        if (formManager.state == FormsManager.formState.idle && curTurn == turns.player)
        {
            if (enemiesOnMap.Length > 0) 
            { 
                enemiesOnMap[curEnemy].state = Enemy.enemyState.playTurn; 
            }

            curTurn = turns.enemy;
        }
    }
}
