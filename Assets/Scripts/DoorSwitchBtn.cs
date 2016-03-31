using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DoorSwitchBtn : MonoBehaviour, IPointerClickHandler {

    DoorSwitch dw;
    FormsManager formManager;
	
	// Update is called once per frame
	void Start () {
        formManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<FormsManager>();
	}

    public void setDoorSwitch(DoorSwitch doorw)
    {
        dw = doorw;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        formManager.curAgent.transform.LookAt(dw.transform.position);
        formManager.curAnimator.SetTrigger("pressBtn");
        formManager.blockMovement1Frame = true;
        dw.unlockDoor();
        Debug.Log("clickkeeed to unlock");
    }
}
