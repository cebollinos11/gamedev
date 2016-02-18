using UnityEngine;
using System.Collections;

public class EnemyGuard : Enemy {

    int actionPointLength = 3;
    [SerializeField] int maxActionPoints = 5;
    [SerializeField] int moveSpeed = 5;

    NavMeshAgent agent;

    [SerializeField] Transform[] waypoints;
    public int waypointID = 0;

    Vector3 curPosition = new Vector3(0, 0, 0);

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
    }
	// Update is called once per frame
	void Update () {
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
}
