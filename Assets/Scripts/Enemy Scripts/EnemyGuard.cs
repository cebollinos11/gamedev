using UnityEngine;
using System.Collections;

public class EnemyGuard : Enemy {

    GameObject player;
    
    int actionPointLength = 3;
    [SerializeField] int maxActionPoints = 5;
    [SerializeField] int moveSpeed = 5;

    NavMeshAgent agent;

    [SerializeField] Transform[] waypoints;
    public int waypointID = 0;

    Vector3 curPosition = new Vector3(0, 0, 0);

    public float fieldOfViewRange = 52f;
    public float visionRange = 30;
    public float spotRange = 20;

    public bool turningEnemy = true;
    bool turnRight = false;

    bool playerkilled;

    public AudioClip turnSound;
    

    UIMaster ui;

    enum enemyState
    {
        stand,
        patrol
    }
    enemyState eState = enemyState.stand;

    [HideInInspector] public bool investigate = false;
    [HideInInspector] public Vector3 investigatePos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        actionpointsLeft = maxActionPoints;
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.FindObjectOfType<UIMaster>();
    }

    IEnumerator Die() {
        ui.ShowGameOver();
        yield return new WaitForSeconds(3f);
        Application.LoadLevel(Application.loadedLevel);
    }

	// Update is called once per frame
	void Update () {

        if (base.state != Enemy.enemyState.inactive)
        {
            if (canSeePlayer() == 2)
            {
                investigate = true;
                Debug.Log("investigating");
            }
            else if (canSeePlayer() == 1)
            {
                //Destroy(player.gameObject);
                if (!playerkilled)
                {
                    playerkilled = true;
                    AudioManager.PlayClip(AudioManager.Instance.DetectSound);

                    StartCoroutine(Die());
                }

                Debug.Log("spotted!");
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
        if(investigate)
        {
            agent.SetDestination(investigatePos);
            curPosition = transform.position;
            eState = enemyState.patrol;
        }
        else if(waypoints.Length > 0)
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
            if (!investigate)
            {
                waypointID++;
            }
            else
            {
                investigate = false;
            }
            eState = enemyState.stand;
        }
    }

    public int canSeePlayer()
    {
        // 0 = cant see him
        // 1 = player spotted
        // 2 = investigate
        int state = 0;

        Vector3 rayDirection = player.transform.position - transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit hit;
        if ((Vector3.Angle(rayDirection, transform.forward)) < fieldOfViewRange)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, visionRange))
            {
                if (hit.transform.tag == "Player")
                {
                    if(Vector3.Distance(hit.transform.position, transform.position) < spotRange)
                    {
                        state = 1; //if player spotted
                        Debug.Log("Player spotteddddddddd!");
                    }
                    else
                    {
                        Debug.Log("investigate");
                        state = 2; //if investigate
                        investigatePos = hit.transform.position;
                    }
                }
            }
        }

        return state;
    }
}
