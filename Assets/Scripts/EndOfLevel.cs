﻿using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {
    FormsManager fm;
    public string NextLevelName;
    bool triggeredAlready;
    [SerializeField]
    AudioClip ElevatorSound;
    void Start() {

        fm = GameObject.FindObjectOfType<FormsManager>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.B))
        {
            GoToNextLevel();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!fm.isSplitted) 
        {
            if (!triggeredAlready) {
                AudioManager.StopAll();
                AudioManager.TurnOffAll();
                AudioManager.PlayClip(ElevatorSound);
                triggeredAlready = true;
                Debug.Log("SHOW LEVEL COMPLETE!");
                CameraShaderManager cam = GameObject.FindObjectOfType<CameraShaderManager>();
                cam.RunWin();
                Debug.Log(Application.loadedLevel);
                Invoke("GoToNextLevel", 3f);

            
            }
        }
        
    }

    void GoToNextLevel()
    {

        Debug.Log("GO TO NEXT LEVEL");
        AudioManager.StopAll();
        AudioManager.TurnOffAll();
        Invoke("GoToNextLevelForReal", 1f);

    }

    void GoToNextLevelForReal()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
