using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    [SerializeField] bool doorUnlocked = false;
    [HideInInspector] public bool doorIsDoingSomething = false;

    GameMaster gamemaster;
    FormsManager formManager;
    GameObject[] playerForms;
    bool splitted = false;

    [SerializeField] Transform door;
    bool showingBtn = false;
    

    [SerializeField] Light light;
    [SerializeField] Color disabledColor, enabledColor;


    [SerializeField] bool complete, physical, digital = false;
    [SerializeField] Transform btn;

    public AudioClip openSound;

    GameObject camFocuser;

	// Use this for initialization
	void Start () {
        camFocuser = Resources.Load("FocusCamera") as GameObject;
        playerForms = GameObject.FindGameObjectsWithTag("Player");
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        formManager = gamemaster.GetComponent<FormsManager>();

        if(complete)
        {
            physical = digital = complete;
        }

        light.color = (doorUnlocked ? enabledColor : disabledColor);

        if (door.GetComponent<Trap>() == null)
        {
            door.gameObject.SetActive(!doorUnlocked);
        }
        btn.GetComponent<DoorSwitchBtn>().setDoorSwitch(this);
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
        }
	}

    void unLockDelayed() {

        doorUnlocked = !doorUnlocked;
        light.color = (doorUnlocked ? enabledColor : disabledColor);

        if(door.GetComponent<Trap>() != null)
        {
            Trap tr = door.GetComponent<Trap>();

            if (!tr.trapActivated)
            {
                door.GetComponent<Trap>().activateMe(9999);
            }
            else
            {
                door.GetComponent<Trap>().turnsLeft = -99;
            }
        }
        else
        {
            door.gameObject.SetActive(!doorUnlocked);
        }
        
        doorIsDoingSomething = false;

        formManager.actionPointsLeft = -1;
        

    }

    public void unlockDoor()
    {
        Instantiate(camFocuser, door.transform.position, Quaternion.identity);
        AudioManager.PlayClip(openSound);

        doorIsDoingSomething = true;
        Invoke("unLockDelayed", 1f);
        

        

        //hideBtn();
    }

    void showBtn()
    {
        if (!btn.gameObject.activeSelf)
        {
            btn.gameObject.SetActive(true);
        }
        showingBtn = true;
    }
    void hideBtn()
    {
        
        if (btn.gameObject.activeSelf)
        {
            btn.gameObject.SetActive(false);
        }

        showingBtn = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(complete)
            {
                if (!splitted)
                {
                    if (!showingBtn)
                    {
                        showBtn();
                    }
                }
            }
            else if(splitted)
            {
                if ((physical && other.gameObject == formManager.spawnedForms[0]) || (digital && other.gameObject == formManager.spawnedForms[1]))
                {
                    if (!showingBtn)
                    {
                        showBtn();
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (showingBtn)
            {
                hideBtn();
            }
        }
    }
}
