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

    Vector3 startingPos = new Vector3(0, 0, 0);
    Quaternion startingRot = new Quaternion(0, 0, 0, 0);

    Vector3 curPosition = new Vector3(0, 0, 0);

    FOV[] fovs;

    public bool turningEnemy = true;
    public float prefTurnAngle = 90;
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

        startingPos = transform.position;
        startingRot = transform.rotation;

        fovs = GetComponentsInChildren<FOV>();

        foreach(FOV fov in fovs)
        {
            fov.init(fieldOfViewRange, visionRange, spotRange);
        }
    }

    IEnumerator Die() {
        transform.LookAt(player.transform);
        AudioManager.TurnOffAll();
        agent.SetDestination(Vector3.Lerp(player.transform.position, transform.position, 0.7f));
        agent.speed = 0f;
        
        CameraShaderManager cam = GameObject.FindObjectOfType<CameraShaderManager>();
        cam.RunDeath();
        ui.ShowGameOver();
        yield return new WaitForSeconds(3f);
        Application.LoadLevel(Application.loadedLevel);
    }

	// Update is called once per frame
	void Update () {
        FOVControl();

        if (playerkilled)
        {
            
            
            
            return;
        }
            

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

            if (seeState == 2 || seeState == 3)
            {
                investigate = true;
                AudioManager.HandleBackgroundMusic();
                investigatePos = (seeState == 2 ? player.transform.position : GameObject.FindGameObjectWithTag("OpticForm").transform.position);
                
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
        if (!investigate && waypoints.Length <= 0 && Vector3.Distance(transform.position, startingPos) > 1)
        {
            eState = enemyState.patrol;
        }
        else
        {
            if (investigate)
            {
                agent.SetDestination(investigatePos);
                curPosition = transform.position;
                eState = enemyState.patrol;
            }
            else if (waypoints.Length > 0)
            {
                if (waypoints.Length - 1 < waypointID)
                {
                    waypointID = 0;
                }
                agent.SetDestination(waypoints[waypointID].position);

                curPosition = transform.position;
                eState = enemyState.patrol;
            }
            else if (turningEnemy)
            {
                if (turnRight)
                {
                    if (transform.rotation != startingRot)
                    {
                        transform.rotation = startingRot;
                    }
                    turnRight = false;
                    actionpointsLeft = 0;
                }
                else
                {
                    transform.Rotate(Vector3.up * -prefTurnAngle);
                    turnRight = true;
                    actionpointsLeft = 0;
                }
                actionpointsLeft--;
            }
            else
            {
                if (transform.rotation != startingRot)
                {
                    transform.rotation = startingRot;
                }
                actionpointsLeft = 0;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.name == "PhysicalForm")
        {

            transform.LookAt(col.gameObject.transform);
            Debug.Log("die");
            AudioManager.PlayClip(AudioManager.Instance.DetectSound);

            StartCoroutine(Die());
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
            if (!investigate && waypoints.Length > 0)
            {
                waypointID++;
                actionpointsLeft = 0;
            }
            else
            {
                if(investigate)
                {
                    actionpointsLeft = 0;
                }
                investigate = false;
                AudioManager.HandleBackgroundMusic();
                foreach(FOV fov in fovs)
                {
                    fov.alert = FOV.alertState.normal;
                }

                agent.SetDestination(startingPos);
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
