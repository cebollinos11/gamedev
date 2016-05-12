using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    [SerializeField]
    AudioClip ClickSound;
    [SerializeField]
    AudioClip ShowDialogSound;

    FormsManager formMan;

    Dialog[] dialogs;

    bool showDialog = false;
    bool dialogShown = false;
    int curDialogID = 0;


    [SerializeField]
        Sprite PlayerSprite;
    [SerializeField]
        Sprite BossSprite;

    [SerializeField] GameObject dialogWindow;
    [SerializeField] Text dialogText, dialogBtnText;
    [SerializeField] Button dialogBtn;


	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = false;

        formMan = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<FormsManager>();

        dialogs = GetComponents<Dialog>();

        Array.Sort(dialogs);


        dialogBtn.onClick.AddListener(() => dialogBtnClicked());
	}
	
	// Update is called once per frame
	void Update () {
	    if(showDialog)
        {
            if(curDialogID >= dialogs.Length-1 && dialogBtnText.text != "End")
            {
                dialogBtnText.text = "End";
            }
            else if (curDialogID < dialogs.Length - 1 && dialogBtnText.text != "Next")
            {
                dialogBtnText.text = "Next";
            }

            if(curDialogID >= dialogs.Length)
            {
                formMan.CONTROLS_IS_ON = true;
                dialogWindow.SetActive(false);

                showDialog = false;
                dialogShown = true;
            }
            else
            {
                if (formMan.CONTROLS_IS_ON)
                {
                    formMan.CONTROLS_IS_ON = false;
                }

                if (!dialogWindow.activeSelf)
                {
                    dialogWindow.SetActive(true);
                    
                }

                if (dialogText.text != dialogs[curDialogID].dialogText)
                {
                    dialogText.text = dialogs[curDialogID].dialogText;
                    Image img = GameObject.Find("talker").GetComponent<Image>();
                    if (dialogs[curDialogID].dialogTalker == Dialog.talker.player)
                        img.sprite = PlayerSprite;
                    if (dialogs[curDialogID].dialogTalker == Dialog.talker.boss)
                        img.sprite = BossSprite;
                }
            }
        }
	}

    void dialogBtnClicked()
    {
        if (showDialog && curDialogID < dialogs.Length)
        {
            curDialogID++;
            formMan.blockMovement1Frame = true;
            AudioManager.PlayClip(ClickSound);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!dialogShown)
        {
            if (other.transform.tag == "Player")
            {
                showDialog = true;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!dialogShown)
        {
            if (other.transform.tag == "Player")
            {
                AudioManager.PlayClip(ShowDialogSound);
                showDialog = true;
            }
        }
    }
}
