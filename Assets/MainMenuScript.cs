using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown) {
            Debug.Log("load first level");
            Application.LoadLevel(1);
        }
	}
}
