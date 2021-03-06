﻿using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

    public GameObject MovablePart;
    public AudioClip soundOpen;


    Vector3 origScale;


	// Use this for initialization
	void Start () {

        origScale = transform.localScale;
	
	}

    public void Open() {
        AudioManager.PlayClip(soundOpen);
        transform.localScale = Vector3.zero;
    }

    public void Close() {
        transform.localScale = origScale;
    }

    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O)) {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Close();
        }
	}
}
