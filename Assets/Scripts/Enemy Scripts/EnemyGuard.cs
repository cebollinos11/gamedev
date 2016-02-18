﻿using UnityEngine;
using System.Collections;

public class EnemyGuard : Enemy {

    Player player;
    
    int actionPointLength = 3;
    [SerializeField] int maxActionPoints = 5;
    [SerializeField] int moveSpeed = 5;

    NavMeshAgent agent;

    [SerializeField] Transform[] waypoints;
    public int waypointID = 0;

    Vector3 curPosition = new Vector3(0, 0, 0);

    public float fieldOfViewRange = 52f;
    public float visionRange = 30;

    public bool turningEnemy = true;
    bool turnRight = false;

    enum enemyState
    {
        stand,
        patrol
    }
    enemyState eState = enemyState.stand;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        actionpointsLeft = maxActionPoints;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	// Update is called once per frame
	void Update () {

        if(canSeePlayer())
        {
            Destroy(player.gameObject);
        }

	    switch(base.state)
        {
            case Enemy.enemyState.idle:
                idle();
                break;
            case Enemy.enemyState.playTurn:
                playTurn();
                break;
            default:
                break;
        }
	}

    void idle()
    {

    }

    void playTurn()
    {
        if(actionpointsLeft <= 0)
        {
            actionpointsLeft = maxActionPoints;
            agent.SetDestination(transform.position);
            eState = enemyState.stand;
            base.state = Enemy.enemyState.idle;
        }
        else
        {
            switch (eState)
            {
                case enemyState.stand:
                    stand();
                    break;
                case enemyState.patrol:
                    patrol();
                    break;
                default:
                    break;
            }
        }
    }

    void stand()
    {
        if(waypoints.Length > 0)
        { 
            if(waypoints.Length-1 < waypointID)
            {
                waypointID = 0;
            }

            agent.SetDestination(waypoints[waypointID].position);

            curPosition = transform.position;
            eState = enemyState.patrol;
        }
        else if(turningEnemy)
        {
            if(turnRight)
            {
                transform.Rotate(Vector3.up * 90);
                turnRight = false;
            }
            else
            {
                transform.Rotate(Vector3.up * -90);
                turnRight = true;
            }
            actionpointsLeft--;
        }
    }
    void patrol()
    {
        if(Vector3.Distance(transform.position, curPosition) >= actionPointLength)
        {
            actionpointsLeft--;
            curPosition = transform.position;
        }

        if (agent.remainingDistance <= 0.01F)
        {
            waypointID++;
            eState = enemyState.stand;
        }
    }

    public bool canSeePlayer()
    {
        Vector3 rayDirection = player.transform.position - transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit hit;

        if ((Vector3.Angle(rayDirection, transform.forward)) < fieldOfViewRange)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, visionRange))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }
}
