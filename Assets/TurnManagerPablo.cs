using UnityEngine;
using System.Collections;

public class TurnManagerPablo : MonoBehaviour {

    int currentEnabled = 0;
    public enemyController[] playerPool;


    void EnableNext() {
        currentEnabled++;
        if (playerPool.Length == currentEnabled)
        {
            currentEnabled = 0;
        }

        for (int i = 0; i < playerPool.Length; i++)
        {
            if (currentEnabled != i)
            {
                playerPool[i].enabled = false;
            }

            else
            {
                playerPool[i].enabled = true;
            }
        }

        Debug.Log(playerPool[currentEnabled].gameObject.name + " turn");
    }

	// Use this for initialization
	void Start () {
        EnableNext();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnableNext();
        }
	
	}
}
