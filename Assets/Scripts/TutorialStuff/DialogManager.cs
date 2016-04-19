using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    FormsManager formMan;

    Dialog[] dialogs;

    bool showDialog = false;
    bool dialogShown = false;
    int curDialogID = 0;

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
                }
            }
        }
	}

    void dialogBtnClicked()
    {
        if (showDialog && curDialogID < dialogs.Length)
        {
            curDialogID++;
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
                showDialog = true;
            }
        }
    }
}
