using UnityEngine;
using System.Collections;

public class UIMaster : MonoBehaviour {


    public GameObject GameOver;

	// Use this for initialization
	void Start () {
        GameOver.SetActive(false);
	}

    public void ShowGameOver() {

        GameOver.SetActive(true);
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
