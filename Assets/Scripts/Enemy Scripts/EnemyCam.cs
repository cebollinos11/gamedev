using UnityEngine;
using System.Collections;

public class EnemyCam : Enemy {

    GameObject player;

    [SerializeField] bool rotatingCamera = false;
    [SerializeField] float FOVUpdateInterval = 0.5F;
    [SerializeField] int fieldOfViewRange = 40;
    [SerializeField] int visionRange = 15;

    EnemyGuard nearestEnemy = null;

    FOV[] fovs;
    //[SerializeField] Transform cam1, cam2;

    bool alertSent = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        fovs = GetComponentsInChildren<FOV>();

        foreach (FOV fov in fovs)
        {
            fov.init(fieldOfViewRange, visionRange);
        }
	}
	
	// Update is called once per frame
	void Update () {
        FOVControl();
        if (base.state != Enemy.enemyState.inactive)
        {
            int seeState = 0;
            foreach (FOV fov in fovs)
            {
                seeState = fov.canSeePlayer();

                if (seeState != 0)
                {
                    break;
                }
            }

            if (seeState != 0)
            {
                Debug.Log("cam: player spotted!");
                if (!alertSent)
                {
                    EnemyGuard[] enemies = GameObject.FindObjectsOfType<EnemyGuard>();

                    foreach (EnemyGuard enemy in enemies)
                    {
                        if (nearestEnemy == null)
                        {
                            nearestEnemy = enemy;
                        }
                        else if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, nearestEnemy.transform.position))
                        {
                            nearestEnemy = enemy;
                        }
                    }

                    if (nearestEnemy != null)
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

                if(nearestEnemy != null && !nearestEnemy.investigate)
                {
                    foreach (FOV fov in fovs)
                    {
                        fov.alert = FOV.alertState.normal;
                    }
                    nearestEnemy = null;
                }
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
        if(rotatingCamera)
        {
            transform.Rotate(Vector3.up * 90);
        }
        base.state = Enemy.enemyState.idle;
    }

    void FOVControl()
    {
        foreach (FOV fov in fovs)
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
