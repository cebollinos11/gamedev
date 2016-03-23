using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormsManager : MonoBehaviour {

    //UI elements
    Transform gameUI;
    Text txtActionPoints;
    Button btnForm0;
    Button btnForm1;
    Button btnCombine;
    //

    //camera follower
    CameraFocuser camFocus;

    [HideInInspector] public GameMaster gamemaster;

    bool isSplitted = false;

    [SerializeField] Transform pointer;
    [SerializeField] TextMesh pointerText;
    [SerializeField] LineRenderer pointerLine;
    
    Vector3 lineEndPos;
    NavMeshAgent curAgent;

    int moveSpeed = 5;
 
    [SerializeField] int maxActionPoints = 5;
    int actionPointsLeft = 5;
    int pointerDistance = 0;

    int curForm = 0;
    [SerializeField] GameObject[] spawnableForms;
    [SerializeField] GameObject[] spawnedForms;

    public enum formState
    {
        idle,
        move
    }
    public formState state = formState.idle;

    LayerMask layerMask;
	// Use this for initialization
	void Start () {
        actionPointsLeft = maxActionPoints;
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        layerMask = 1 << LayerMask.NameToLayer("Walkable");

        

        //ui
        gameUI = GameObject.FindGameObjectWithTag("gameUI").transform;

        txtActionPoints = gameUI.FindChild("txtActionPoints").GetComponent<Text>();
        btnForm0 = gameUI.FindChild("btnPhysical").GetComponent<Button>();
        btnForm1 = gameUI.FindChild("btnDigital").GetComponent<Button>();
        btnCombine = gameUI.FindChild("btnCombine").GetComponent<Button>();

        btnForm0.onClick.AddListener(() => formBtnClicked(0));
        btnForm1.onClick.AddListener(() => formBtnClicked(1));
        btnCombine.onClick.AddListener(() => combineFormsBtnClicked());

        GameObject physical = spawnedForms[0];
        spawnedForms = new GameObject[spawnableForms.Length];
        spawnedForms[0] = physical;
        //

        curAgent = spawnedForms[0].GetComponent<NavMeshAgent>();
        pointerLine.SetPosition(0, spawnedForms[0].transform.position);


        camFocus = Object.FindObjectOfType<CameraFocuser>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gamemaster.curTurn == GameMaster.turns.player)
        {
            switch (state)
            {
                case formState.idle:
                    idle();
                    break;
                case formState.move:
                    move();
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (pointerLine.gameObject.activeSelf) pointerLine.gameObject.SetActive(false);
            if (pointerText.gameObject.activeSelf) pointerText.gameObject.SetActive(false);
            if (pointer.gameObject.activeSelf) pointer.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            formBtnClicked(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            formBtnClicked(1);
        }

        if(txtActionPoints.text != "Actionpoints: "+ (actionPointsLeft+1))
        {
            txtActionPoints.text = "Actionpoints: " + (actionPointsLeft+1);
        }
	}

    void idle()
    {
        if (actionPointsLeft+1 > 0)
        {
            resetPointer();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
            {
                Vector3 hitPoint = hit.point;

                //the direction of the destination according to the player(used for showing the pointer)
                Vector3 heading = hitPoint - spawnedForms[curForm].transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;

                RaycastHit hitt;
                if (Physics.Raycast(spawnedForms[curForm].transform.position, heading, out hitt, distance))
                {
                    if (hitt.transform.gameObject.layer != LayerMask.NameToLayer("Walkable"))
                    {
                        hitPoint = hitt.point;
                    }
                }

                //end point of line shown (also destination of the player movement)
                lineEndPos = hitPoint;

                //distance from point to player
                pointerDistance = Mathf.FloorToInt(Vector3.Distance(spawnedForms[curForm].transform.position, hitPoint) / moveSpeed);

                if (pointerDistance > actionPointsLeft) //don't move more than action points left
                {
                    pointerDistance = actionPointsLeft;
                    lineEndPos = spawnedForms[curForm].transform.position + (direction * ((pointerDistance + 1) * moveSpeed));
                }

                //show actionpoints used if desiding to move
                pointerText.text = "" + (pointerDistance + 1);
                pointerText.transform.LookAt(Camera.main.transform.position);
                //

                //set pointer line end pos
                pointerLine.SetPosition(1, lineEndPos);

                //set position of "pointer"
                pointer.position = spawnedForms[curForm].transform.position + (direction * (pointerDistance * moveSpeed));
            }

            if (Input.GetMouseButtonDown(1) && Vector3.Distance(spawnedForms[curForm].transform.position, lineEndPos) > 2)
            {
                curAgent.SetDestination(lineEndPos);
                pointerLine.gameObject.SetActive(false);
                pointerText.gameObject.SetActive(false);
                pointer.gameObject.SetActive(false);
                actionPointsLeft -= pointerDistance+1;
                pointerDistance = 0;
                state = formState.move;
                Debug.Log(spawnedForms[curForm]);
                camFocus.TargetToFollow = spawnedForms[curForm].transform;
            }
        }
    }
    void move()
    {
        if (curAgent.remainingDistance < 0.01F)
        {
            camFocus.TargetToFollow = null;
            resetPointer();
            state = formState.idle;
        }
    }

    public void resetTurn()
    {
        actionPointsLeft = maxActionPoints;
        pointerLine.gameObject.SetActive(true);
        pointerText.gameObject.SetActive(true);
        pointer.gameObject.SetActive(true);
    }

    public void resetPointer()
    {
        if (!pointer.gameObject.activeSelf)
        {
            pointer.gameObject.SetActive(true);
        }
        if (!pointerLine.gameObject.activeSelf)
        {
            pointerLine.gameObject.SetActive(true);
            pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);
        }
        if (!pointerText.gameObject.activeSelf)
        {
            pointerText.gameObject.SetActive(true);
            pointerText.text = "" + 0;
        }
    }

    void formBtnClicked(int formID)
    {
        if (gamemaster.curTurn == GameMaster.turns.player)
        {
            state = formState.idle;

            if (spawnedForms.Length > formID && spawnedForms[formID] != null)
            {
                curForm = formID;
                curAgent = spawnedForms[curForm].GetComponent<NavMeshAgent>();
                pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);
                
            }
            else if (!isSplitted)
            {
                if (formID == 1)
                {
                    TryToSpawnDigital();
                }
            }
        }
    }

    void combineFormsBtnClicked()
    {
        if (isSplitted)
        {
            bool allIsClose = true;

            foreach (GameObject form in spawnedForms)
            {
                if (Vector3.Distance(form.transform.position, spawnedForms[0].transform.position) > 5)
                {
                    allIsClose = false;
                    break;
                }
            }

            if (allIsClose)
            {
                curForm = 0;
                curAgent = spawnedForms[curForm].GetComponent<NavMeshAgent>();
                pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);

                for (int i = 1; i < spawnedForms.Length; i++)
                {
                    Destroy(spawnedForms[i]);
                    spawnedForms[i] = null;
                }

                isSplitted = false;
            }
            else
            {
                Debug.Log("all forms are not close to each other!");
            }
        }
        else
        {
            Debug.Log("you need to split before you can combine!");
        }
    }

    void TryToSpawnDigital()
    {
        //try to spawm
        ElectronicUnit[] terminals = GameObject.FindObjectsOfType<ElectronicUnit>();

        Debug.Log("found: "+terminals.Length);
        bool isTerminalClose = false;
        int terminalID = -1;

        for (int i = 0; i < terminals.Length; i++)
        {
            Debug.Log("dist: " + Vector3.Distance(spawnedForms[0].transform.position, terminals[i].transform.position) + " usable: " + terminals[i].isUsable);
            if (Vector3.Distance(spawnedForms[0].transform.position, terminals[i].transform.position) < 4 && terminals[i].isUsable)
            {
                Debug.Log("can split");
                isTerminalClose = true;
                terminalID = i;
            }
        }

        if (isTerminalClose)
        {
            Debug.Log("EXPLODEEEEEE");
            spawnedForms[1] = Instantiate(spawnableForms[1], terminals[terminalID].getSpawnPoint(), Quaternion.identity) as GameObject;

            curForm = 1;
            curAgent = spawnedForms[curForm].GetComponent<NavMeshAgent>();
            pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);
            isSplitted = true;
        }
        else
        {
            Debug.Log("cannot split now into terminal form");
        }
    }

}
