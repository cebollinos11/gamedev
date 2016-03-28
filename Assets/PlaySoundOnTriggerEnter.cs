﻿using UnityEngine;
using System.Collections;

public class PlaySoundOnTriggerEnter : MonoBehaviour {

    public AudioClip clipOnEnter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {

        Debug.Log(col);
        AudioManager.PlayClip(clipOnEnter);
    
    }

    void OnTriggerExit(Collider col)
    {

        Debug.Log(col);
        AudioManager.PlayClip(clipOnEnter);

    }
}
