using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameMaster : MonoBehaviour {

    //UI elements
    Transform gameUI;
    Text txtCurTurn;
    Button btnEndTurn;
    //


    [HideInInspector]public UIMaster ui;

    [HideInInspector] public int turnNumber = 0;
    public enum turns
    {
        player,
        enemy
    }
    public turns curTurn = turns.player;

    [SerializeField] Enemy[] enemiesOnMap;
    bool enemyTurnsPlayed = false;

    FormsManager formManager;

    CameraFocuser camFocus;

    public AudioClip soundEndTurn;
    public AudioClip soundStartTurn;

	// Use this for initialization
	void Start () {
        formManager = GetComponent<FormsManager>();

        gameUI = GameObject.FindGameObjectWithTag("gameUI").transform;
        txtCurTurn = gameUI.FindChild("txtTurn").GetComponent<Text>();
        btnEndTurn = gameUI.FindChild("btnEndTurn").GetComponent<Button>();
        btnEndTurn.onClick.AddListener(() => endTurnBtnClicked());

        ui = GameObject.FindObjectOfType<UIMaster>();
        

        //grab all enemies from map
        enemiesOnMap = GameObject.FindObjectsOfType<Enemy>();

        camFocus = Object.FindObjectOfType<CameraFocuser>();

        //check for checkpoints
        CheckPointManager.init();
        if (CheckPointManager.GetLevel() == Application.loadedLevel)
        {
            Debug.Log("Using checkpoint");
            GameObject.FindGameObjectWithTag("Player").transform.position = CheckPointManager.GetPosition();
            camFocus.GoTo("PhysicalForm");
            
        }
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
        
        //ui.HideWait();
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
            if(!enemyTurnsPlayed)
            {
                foreach(Enemy en in enemiesOnMap)
                {
                    if (en.state != Enemy.enemyState.inactive)
                    {
                        en.state = Enemy.enemyState.playTurn;
                        Debug.Log("really start: " + en.name);
                    }
                    Debug.Log("start: " + en.name);
                }

                enemyTurnsPlayed = true;
            }
            else
            {
                bool everythingGood = true;

                foreach (Enemy en in enemiesOnMap)
                {
                    if(en.state == Enemy.enemyState.playTurn)
                    {
                        everythingGood = false;
                        break;
                    }
                }

                if(everythingGood)
                {
                    Debug.Log("my turn");
                    AudioManager.HandleBackgroundMusic();
                    enemyTurnsPlayed = false;
                    curTurn = turns.player;
                    Debug.Log("Player turn start");
                    ui.HideWait();
                    //ui.Flash.FlashIt(Color.white);
                    //AudioManager.PlayClip(soundStartTurn);
                    formManager.resetTurn();
                }
            }


            /*

            if (enemiesOnMap[curEnemy].state == Enemy.enemyState.idle || enemiesOnMap[curEnemy].state == Enemy.enemyState.inactive)
            {
                if (enemiesOnMap.Length - 1 <= curEnemy)
                {
                    
                }
                else
                {
                    //AudioManager.PlayClip(enemiesOnMap[curEnemy].);
                    
                    curEnemy++;
                    if (enemiesOnMap[curEnemy].state != Enemy.enemyState.inactive)
                    {
                        enemiesOnMap[curEnemy].state = Enemy.enemyState.playTurn;
                    }
                    //Debug.Log("2curEnemy = " + curEnemy.ToString() + " plays turn = " + enemiesOnMap[curEnemy].gameObject.name);
                }
            }*/
        }
        else
        {
            curTurn = turns.player;
            formManager.resetTurn();
        }
    }

    public void endTurnBtnClicked()
    {
        AudioManager.PlayClip(soundEndTurn);
        ui.ShowWait();
        //ui.Flash.FlashIt(Color.red);
        
        //if the end turn btn is clicked and its currently the players turn, then change turn to enemy
        if (formManager.state == FormsManager.formState.idle && curTurn == turns.player)
        {
            if (enemiesOnMap.Length > 0) 
            {/*
                if (enemiesOnMap[curEnemy].state != Enemy.enemyState.inactive)
                {
                    //Debug.Log("set to playturn for " + enemiesOnMap[curEnemy].gameObject.name);
                    enemiesOnMap[curEnemy].state = Enemy.enemyState.playTurn;
                }*/

                //added by pablo

                //for (int i = 0; i < enemiesOnMap.Length; i++)
                //{
                //    enemiesOnMap[curEnemy].state = Enemy.enemyState.playTurn;
                //}
            }

            turnNumber++;
            curTurn = turns.enemy;
        }
    }
}
