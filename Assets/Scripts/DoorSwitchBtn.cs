using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DoorSwitchBtn : MonoBehaviour, IPointerClickHandler {

    DoorSwitch dw;
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setDoorSwitch(DoorSwitch doorw)
    {
        dw = doorw;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        dw.unlockDoor();
        Debug.Log("clickkeeed");
    }
}
