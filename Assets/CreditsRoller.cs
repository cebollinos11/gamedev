using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsRoller : MonoBehaviour {

    [SerializeField] float speed = 5;
    [SerializeField] RectTransform panel;
	
	// Update is called once per frame
	void Update () {
        //panel.offsetMax = new Vector2(panel.offsetMax.x, -864);
        Debug.Log("sdf" + panel.position.y);
        if (panel.position.y < 1504) 
        {
            
            panel.position = new Vector2(panel.position.x, panel.position.y + speed);
        }

        if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.B))
        {
            Application.LoadLevel(0);
        }
	}
}
