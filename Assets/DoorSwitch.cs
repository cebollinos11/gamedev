using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    public DoorManager DoorToOpen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("ahasdhashd");
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "CableForm")
        {
            Debug.Log("opening door");
            DoorToOpen.Open();
        }
    
    }


    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "CableForm")
        {
            Debug.Log("closing door");
            DoorToOpen.Close();
        }

    }
}
