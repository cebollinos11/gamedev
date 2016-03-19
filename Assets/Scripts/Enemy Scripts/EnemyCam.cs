using UnityEngine;
using System.Collections;

public class EnemyCam : Enemy {

    GameObject player;

    [SerializeField] bool rotatingCamera = false;
    [SerializeField] public float fieldOfViewRange = 40f;
    [SerializeField] public float visionRange = 15;

    [SerializeField] Transform cam1, cam2;

    bool alertSent = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if(canSeePlayer())
        {
            Debug.Log("cam: player spotted!");
            if(!alertSent)
            {
                EnemyGuard[] enemies = GameObject.FindObjectsOfType<EnemyGuard>();
                EnemyGuard nearestEnemy = null;

                foreach(EnemyGuard enemy in enemies)
                {
                    if(nearestEnemy == null)
                    {
                        nearestEnemy = enemy;
                    }
                    else if(Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, nearestEnemy.transform.position))
                    {
                        nearestEnemy = enemy;
                    }
                }

                if(nearestEnemy != null)
                {
                    nearestEnemy.investigate = true;
                    nearestEnemy.investigatePos = player.transform.position;
                }

                alertSent = true;
            }
        }
        else
        {
            if (alertSent) alertSent = false;
        }

        switch (base.state)
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
        if(rotatingCamera)
        {
            transform.Rotate(Vector3.up * 90);
        }
        base.state = Enemy.enemyState.idle;
    }

    public bool canSeePlayer()
    {
        bool playerSpotted = false;

        Vector3 rayDirection = player.transform.position - transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit hit;

        if ((Vector3.Angle(rayDirection, cam1.forward)) < fieldOfViewRange || (Vector3.Angle(rayDirection, cam2.forward)) < fieldOfViewRange)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, visionRange))
            {
                if (hit.transform.tag == "Player")
                {
                    playerSpotted = true;
                }
            }
        }

        return playerSpotted;
    }
}
