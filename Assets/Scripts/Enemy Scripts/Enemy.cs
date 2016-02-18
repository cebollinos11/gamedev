using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int actionpointsLeft = 3;

    public enum enemyState
    {
        idle,
        playTurn
    }
    public enemyState state = enemyState.idle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}