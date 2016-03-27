using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    GameMaster gamemaster;
    FormsManager formManager;
    GameObject[] playerForms;
    bool splitted = false;

    [SerializeField] Transform door;
    bool showingBtn = false;
    bool doorUnlocked = false;

    [SerializeField] Light light;
    [SerializeField] Color disabledColor, enabledColor;


    [SerializeField] bool complete, physical, digital = false;
    [SerializeField] Transform btn;

    public AudioClip openSound;

	// Use this for initialization
	void Start () {
        playerForms = GameObject.FindGameObjectsWithTag("Player");
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        formManager = gamemaster.GetComponent<FormsManager>();

        if(complete)
        {
            physical = digital = complete;
        }

        light.color = (doorUnlocked ? enabledColor : disabledColor);

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

    public void unlockDoor()
    {
        AudioManager.PlayClip(openSound);
        doorUnlocked = true;
        light.color = enabledColor;
        door.gameObject.SetActive(false);

        hideBtn();
    }

    void showBtn()
    {
        if (!doorUnlocked)
        {
            Debug.Log("show btn");
            if (!btn.gameObject.activeSelf)
            {
                btn.gameObject.SetActive(true);
            }
            showingBtn = true;
        }
    }
    void hideBtn()
    {
        Debug.Log("hide btn");
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
