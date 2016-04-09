using UnityEngine;
using System.Collections;

public class OpticForm : MonoBehaviour {
    GameMaster gm;
    FormsManager fm;
    Animator animator;
    ParticleSystem particles;
    Transform myTransform;

    [SerializeField] int moveSpeed = 150;

    Vector3 requestedPos;

    [SerializeField] LayerMask LOSEffectedBy;

    enum states
    {
        stand,
        move
    }
    states state = states.stand;
	// Use this for initialization
	void Awake () {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        fm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<FormsManager>();

        animator = GetComponent<Animator>();
        particles = gameObject.transform.FindChild("particles").GetComponent<ParticleSystem>();

        myTransform = transform;
        requestedPos = myTransform.position;
	}
	
	// Update is called once per frame
	void Update () {

        switch(state)
        {
            case states.stand:
                stand();
                break;
            case states.move:
                move();
                break;
            default:
                break;
        }
	}

    void stand()
    {
        if(Vector3.Distance(myTransform.position, requestedPos) > 2)
        {
            state = states.move;
        }
        else
        {
            if (gm.curTurn == GameMaster.turns.player)
            {
                RaycastHit hit;

                Vector3 dir = (fm.spawnedForms[0].transform.position - transform.position).normalized;

                if (Physics.Raycast(transform.position, dir, out hit))
                {
                    if (hit.transform.gameObject != fm.spawnedForms[0])
                    {
                        Destroy(gameObject);
                    }
                }
            }

            if (particles.isPlaying)
            {
                particles.Stop();
            }

            if (animator.GetBool("move"))
            {
                animator.SetBool("move", false);
            }
        }
    }

    void move()
    {
        if (Vector3.Distance(myTransform.position, requestedPos) <= 1)
        {
            state = states.stand;
        }
        else
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }

            if(!animator.GetBool("move"))
            {
                animator.SetBool("move", true);
            }

            myTransform.LookAt(requestedPos);
            myTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void setNewPos(Vector3 pos)
    {
        requestedPos = new Vector3(
            pos.x, 
            myTransform.position.y, 
            pos.z);

        if (Vector3.Distance(myTransform.position, requestedPos) > 2)
        {
            state = states.move;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
