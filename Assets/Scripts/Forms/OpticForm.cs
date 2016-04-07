using UnityEngine;
using System.Collections;

public class OpticForm : MonoBehaviour {
    GameMaster gm;
    FormsManager fm;
    NavMeshAgent agent;
    Animator animator;
    ParticleSystem particles;
	// Use this for initialization
	void Start () {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        fm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<FormsManager>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        particles = gameObject.transform.FindChild("particles").GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        if(agent.remainingDistance < 0.1F)
        {
            agent.Stop();
            if (particles.isPlaying)
            {
                particles.Stop();
            }

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
        }
        else
        {
            agent.Resume();
            if(!particles.isPlaying)
            {
                particles.Play();
            }
        }
	}

    public void setNewPos(Vector3 pos)
    {
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if(animator == null)
        {
            GetComponent<Animator>();
        }

        agent.SetDestination(pos);
    }
}
