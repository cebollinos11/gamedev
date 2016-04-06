using UnityEngine;
using System.Collections;

public class EnemyGuard : Enemy {

    GameObject player;
    
    int actionPointLength = 3;
    [SerializeField] int maxActionPoints = 5;
    [SerializeField] int moveSpeed = 5;
    [SerializeField] float FOVUpdateInterval = 0.5F;
    [SerializeField] int fieldOfViewRange = 80, visionRange = 30, spotRange = 20;

    NavMeshAgent agent;

    [SerializeField] Transform[] waypoints;
    public int waypointID = 0;

    Vector3 curPosition = new Vector3(0, 0, 0);

    FOV[] fovs;

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

    public bool investigate = false;
    [HideInInspector] public Vector3 investigatePos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        actionpointsLeft = maxActionPoints;
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.FindObjectOfType<UIMaster>();

        fovs = GetComponentsInChildren<FOV>();

        foreach(FOV fov in fovs)
        {
            fov.init(fieldOfViewRange, visionRange, spotRange);
        }

        InvokeRepeating("FOVControl", 0, FOVUpdateInterval);
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
            int seeState = 0;
            foreach(FOV fov in fovs)
            {
                seeState = fov.canSeePlayer();

                if(seeState != 0)
                {
                    break;
                }
            }

            if (seeState == 2)
            {
                investigate = true;
                investigatePos = player.transform.position;
                Debug.Log("investigating");
            }
            else if (seeState == 1)
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
                actionpointsLeft = 0;
            }
            else
            {
                transform.Rotate(Vector3.up * -90);
                turnRight = true;
                actionpointsLeft = 0;
            }
            actionpointsLeft--;
        }
        else
        {
            actionpointsLeft = 0;
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
                foreach(FOV fov in fovs)
                {
                    fov.alert = FOV.alertState.normal;
                }
            }
            eState = enemyState.stand;
        }
    }

    void FOVControl()
    {
        foreach(FOV fov in fovs)
        {
            if (state == Enemy.enemyState.inactive)
            {

                fov.clearMesh();
            }
            else
            {
                fov.genFOV();
            }
        }
        
    }
}
