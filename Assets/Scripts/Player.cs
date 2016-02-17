using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] Transform pointer;
    [SerializeField] TextMesh pointerText;
    [SerializeField] LineRenderer pointerLine;
    Vector3 lineEndPos;
    NavMeshAgent agent;

    int moveSpeed = 5;
    int actionPointsLeft = 5;
	
    enum playerState
    {
        idle,
        move
    }
    [SerializeField] playerState state = playerState.idle;

    LayerMask lm;
    void Start()
    {
        lm = 1 << LayerMask.NameToLayer("Walkable");
        agent = GetComponent<NavMeshAgent>();
    }

	// Update is called once per frame
	void Update () {
        switch(state)
        {
            case playerState.idle:
                idle();
                break;
            case playerState.move:
                move();
                break;
            default:
                break;
        }
	}

    void idle()
    {   
        if (actionPointsLeft > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, lm))
            {
                Vector3 hitPoint = hit.point;

                //the direction of the destination according to the player(used for showing the pointer)
                Vector3 heading = hitPoint - transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;

                RaycastHit hitt;
                if (Physics.Raycast(transform.position, heading, out hitt, distance))
                {
                    if(hitt.transform.tag == "wall")
                    {
                        hitPoint = hitt.point;
                    }
                }

                //end point of line shown (also destination of the player movement)
                lineEndPos = hitPoint;

                //distance from point to player
                int pointerDistance = Mathf.FloorToInt(Vector3.Distance(transform.position, hitPoint) / moveSpeed);

                if (pointerDistance > actionPointsLeft) //don't move more than action points left
                {
                    pointerDistance = actionPointsLeft;
                    lineEndPos = transform.position + (direction * ((pointerDistance + 1) * moveSpeed));
                }

                //show actionpoints used if desiding to move
                pointerText.text = "" + (pointerDistance+1);
                pointerText.transform.LookAt(Camera.main.transform.position);
                //

                //set pointer line end pos
                pointerLine.SetPosition(1, lineEndPos);

                //set position of "pointer"
                pointer.position = transform.position + (direction * (pointerDistance * moveSpeed));
            }

            if (Input.GetMouseButtonDown(0) && Vector3.Distance(transform.position,lineEndPos) > 2)
            {
                agent.SetDestination(lineEndPos);
                pointerLine.gameObject.SetActive(false);
                pointerText.gameObject.SetActive(false);
                pointer.gameObject.SetActive(false);
                state = playerState.move;
            }
        }
    }
    void move()
    { 
        if (agent.remainingDistance < 0.01F)
        {
            if (!pointer.gameObject.activeSelf)
            {
                pointer.gameObject.SetActive(true);
            }
            if (!pointerLine.gameObject.activeSelf)
            {
                pointerLine.gameObject.SetActive(true);
                pointerLine.SetPosition(0, transform.position);
            }
            if (!pointerText.gameObject.activeSelf)
            {
                pointerText.gameObject.SetActive(true);
                pointerText.text = "" + 0;
            }

            state = playerState.idle;
        }
    }
}