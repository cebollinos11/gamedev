using UnityEngine;
using System.Collections;

public class EnemyGuard : Enemy {

    int moveSpeed = 5;
    NavMeshAgent agent;

    enum enemyState
    {
        idle,
        move
    }
    [SerializeField] enemyState state = enemyState.idle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    switch(state)
        {
            case enemyState.idle:
                idle();
                break;
            case enemyState.move:
                move();
                break;
            default:
                break;
        }
	}

    void idle()
    {

    }

    void move()
    {

    }
}
