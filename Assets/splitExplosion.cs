﻿using UnityEngine;
using System.Collections;

public class splitExplosion : MonoBehaviour {

    public float timeToLive;

	// Use this for initialization
	void Start () {

        Debug.Log("explo nigga");
	
	}
	
	// Update is called once per frame
	void Update () {

        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
            Destroy(gameObject);

        transform.localScale += Vector3.Scale(transform.localScale * Time.deltaTime , new Vector3(0.5f,1f,0.5f));
	}
}