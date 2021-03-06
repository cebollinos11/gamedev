﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormsManager : MonoBehaviour {

    [HideInInspector] public bool CONTROLS_IS_ON = true;
    //UI elements
    Transform gameUI;
    Text txtActionPoints;
    Button btnForm0;
    Button btnForm1;
    Button btnForm2; GameObject txtOptic;
    Button btnCombine;
    //

    //camera follower
    CameraFocuser camFocus;

    [HideInInspector] public GameMaster gamemaster;

    [HideInInspector] public bool isSplitted = false;

    [SerializeField] Transform pointer;
    [SerializeField] Image pointerAPIndic, pointerAPLeftIndic;
    [SerializeField] LineRenderer pointerLine;

    MeshRenderer pointerRenderer;
    
    Vector3 lineEndPos;
    [HideInInspector] public NavMeshAgent curAgent;
    [HideInInspector] public Animator curAnimator;

    int moveSpeed = 5;
 
    [SerializeField] int maxActionPoints = 5;
    public int actionPointsLeft = 5;
    int pointerDistance = 0;

    public int curForm = 0;
    [SerializeField] GameObject[] spawnableForms;
    public GameObject[] spawnedForms;

    public AudioClip playerWalkSound;
    public AudioClip digitalSplitSound;

    //public GameObject splitExplosion;

    private float elapsed;
    private NavMeshPath path;

    public enum formState
    {
        idle,
        move
    }
    public formState state = formState.idle;

    TextureHiderManager textureHiderManager;

    public bool blockMovement1Frame;
    

    LayerMask layerMask;
	
    //optic form
    [SerializeField] GameObject opticFormPrefab;
    GameObject spawnedOpticForm;
    OpticForm opticFormScript;
    
    bool opticControl = false;
    //
    

    //unstucker
    int unstuckWait = 70;
    int curUnstuckWait;

    Vector3 curFormLoc;
    // /unstucker

    [SerializeField] Color physLineColor, digiLineColor;

    [SerializeField]
    AudioClip OpticCastSound,OpticCastedSound;


    // Use this for initialization
	void Start () {
        actionPointsLeft = maxActionPoints;
        textureHiderManager = GameObject.FindObjectOfType<TextureHiderManager>();
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        layerMask = 1 << LayerMask.NameToLayer("Walkable");

        spawnedForms[0] = GameObject.FindGameObjectWithTag("Player");
        

        //ui
        gameUI = GameObject.FindGameObjectWithTag("gameUI").transform;

        
        txtActionPoints = gameUI.FindChild("txtActionPoints").GetComponent<Text>();
        btnForm0 = gameUI.FindChild("btnPhysical").GetComponent<Button>();
        btnForm1 = gameUI.FindChild("btnDigital").GetComponent<Button>();
        btnForm2 = gameUI.FindChild("btnOptic").GetComponent<Button>(); txtOptic = gameUI.FindChild("txtOpticControl").gameObject;
        btnCombine = gameUI.FindChild("btnCombine").GetComponent<Button>();

        btnForm2.gameObject.SetActive(gamemaster.opticFormIsAvailable); 

        btnForm0.onClick.AddListener(() => formBtnClicked(0));
        btnForm1.onClick.AddListener(() => formBtnClicked(1));
        btnForm2.onClick.AddListener(() => opticControl = true);
        btnCombine.onClick.AddListener(() => combineFormsBtnClicked());

        GameObject physical = spawnedForms[0];
        spawnedForms = new GameObject[spawnableForms.Length];
        spawnedForms[0] = physical;
        //

        curAgent = spawnedForms[0].GetComponent<NavMeshAgent>();
        curAnimator = spawnedForms[0].GetComponent<Animator>();
        pointerLine.SetPosition(0, spawnedForms[0].transform.position);


        camFocus = Object.FindObjectOfType<CameraFocuser>();
        //camFocus.TargetToFollow = spawnedForms[0].transform;

        elapsed = 0f;
        path = new NavMeshPath();


        //Optic form
        if(spawnedOpticForm != null) opticFormScript = spawnedOpticForm.GetComponent<OpticForm>();
        //

        //unstucker
        curUnstuckWait = unstuckWait;
        // /unstucker


        pointerRenderer = pointer.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(txtOptic.activeSelf != opticControl)
        {
            txtOptic.SetActive(opticControl);
        }

        if (!opticControl)
        {
            
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
                if (pointerAPIndic.gameObject.activeSelf) pointerAPIndic.gameObject.SetActive(false);
                if (pointerAPLeftIndic.gameObject.activeSelf) pointerAPLeftIndic.gameObject.SetActive(false);
                if (pointer.gameObject.activeSelf) pointer.gameObject.SetActive(false);
            }

            if (CONTROLS_IS_ON)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    formBtnClicked(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    formBtnClicked(1);
                }
                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    combineFormsBtnClicked();
                }
                if (gamemaster.opticFormIsAvailable && Input.GetKeyDown(KeyCode.Alpha3))
                {
                    opticControl = true;
                }
            }
            if (txtActionPoints.text != "Actionpoints: " + (actionPointsLeft + 1))
            {
                txtActionPoints.text = "Actionpoints: " + (actionPointsLeft + 1);
            }
        }
        else
        {

            CameraShaderManager csm = Camera.main.GetComponent<CameraShaderManager>();

            if (!csm.isOptic)
            {
                Debug.Log("OPTICCCCCCCCCCCCCCCCCCC");
                AudioManager.PlayClip(OpticCastSound);
            }
            csm.SetOptic();
               
            
            if (Input.GetMouseButtonUp(0) && CONTROLS_IS_ON)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Walkable")
                    {
                        RaycastHit hitt;

                        Vector3 startPos = new Vector3(spawnedForms[0].transform.position.x, spawnedForms[0].transform.position.y+5, spawnedForms[0].transform.position.z);

                        Vector3 dir = (hit.point - startPos).normalized;

                        if (Physics.Raycast(startPos, dir, out hitt))
                        {
                            if (Vector3.Distance(hitt.point, hit.point) < 0.1f)
                            {
                                AudioManager.PlayClip(OpticCastedSound);
                                //Debug.Log("Can call the optic here! yay!");
                                if (spawnedOpticForm != null)
                                {
                                    if (opticFormScript != null)
                                    {
                                        opticFormScript.setNewPos(hit.point);
                                        Camera.main.GetComponent<CameraShaderManager>().RemoveOptic();
                                        
                                    }
                                }
                                else
                                {
                                    spawnedOpticForm = Instantiate(opticFormPrefab, spawnedForms[0].transform.position, Quaternion.identity) as GameObject;
                                    opticFormScript = spawnedOpticForm.GetComponent<OpticForm>();
                                    opticFormScript.setNewPos(hit.point);
                                    Camera.main.GetComponent<CameraShaderManager>().RemoveOptic();
                                }

                                opticControl = false;
                            }
                            else
                            {
                                //Debug.Log("cant see it");
                                //Debug.Log(hitt.point);
                                //Debug.Log(hit.point);
                            }
                        }
                        else
                        {
                            //Debug.Log("raycast no");
                        }
                    }
                    else
                    {
                        //Debug.Log("Can't call the optic here!");
                    }
                }
            }
        }
	}

    float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }

    void idle()
    {
        if (CONTROLS_IS_ON && (curForm == 1 || actionPointsLeft + 1 > 0))
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
                        //hitPoint = hitt.point;
                        //Debug.Log("hitpoint changed!!");
                    }
                }

                //end point of line shown (also destination of the player movement)
                lineEndPos = hitPoint;

                //distance from point to player
                
                pointerDistance = Mathf.FloorToInt(Vector3.Distance(spawnedForms[curForm].transform.position, hitPoint) / moveSpeed);


                if (curForm != 1)
                {
                    if (pointerDistance > actionPointsLeft) //don't move more than action points left
                    {
                        pointerDistance = actionPointsLeft;
                        lineEndPos = spawnedForms[curForm].transform.position + (direction * ((pointerDistance + 1) * moveSpeed));
                    }
                }
                //show actionpoints used if desiding to move
                //pointerAPIndic.text = "" + (pointerDistance + 1);

                //Debug.Log("yyy:" + ((pointerDistance + 1) / (float)(maxActionPoints + 1)));
                pointerAPLeftIndic.fillAmount = ((actionPointsLeft + 1) / (float)(maxActionPoints + 1));

                pointerAPIndic.fillAmount = ((pointerDistance + 1) / (float)(maxActionPoints + 1));


                //pointerAPIndic.fillAmount = (((actionPointsLeft + 1) / (float)(maxActionPoints + 1)) - ((pointerDistance-1) / (float)(actionPointsLeft + 1)));
                //pointerText.transform.LookAt(Camera.main.transform.position);
                //

                //added by pablo
               
                elapsed += Time.deltaTime;
                if (elapsed > 0.01f)
                {
                    elapsed = 0f ;
                    NavMesh.CalculatePath(spawnedForms[curForm].transform.position, lineEndPos, NavMesh.AllAreas, path);
                    

                    //resize line renderer
                    pointerLine.SetVertexCount(path.corners.Length);                  


                    for (int i = 0; i < path.corners.Length ; i++)
                    {

                        pointerLine.SetPosition(i , path.corners[i]);


                    }


                }
                
                    
                    //Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);	

                //set pointer line end pos
                //Debug.Log(path.corners.Length);
                //if (path.corners.Length > 1)
                //{
                //    //pointerLine.SetPosition(path.corners.Length - 1, lineEndPos);
                //}
                    

                //set position of "pointer"


                //fix bug of super trip

                pointerDistance = Mathf.FloorToInt( PathLength(path) / moveSpeed);


                if (curForm != 1)
                {
                    if (pointerDistance > actionPointsLeft) //don't move more than action points left
                    {
                        pointerAPIndic.fillAmount = 0;
                        //pointerText.text = "";
                        pointerLine.SetVertexCount(0);
                        pointerDistance = 0;
                        lineEndPos = spawnedForms[curForm].transform.position;
                    }
                }


                
                pointer.position = spawnedForms[curForm].transform.position + (direction * (pointerDistance * moveSpeed));

                //pointer.rotation = hit.transform.rotation;

                if (path.corners.Length - 1 > 0)
                {
                    pointer.position = path.corners[path.corners.Length - 1];

                    if (!pointerRenderer.enabled)
                    {
                        pointerRenderer.enabled = true;
                    }
                    if (!pointerAPIndic.enabled)
                    {
                        pointerAPIndic.enabled = true;
                        
                    }
                    if (!pointerAPLeftIndic.enabled)
                    {
                        pointerAPLeftIndic.enabled = true;

                    }
                }
                else
                {
                    if (pointerRenderer.enabled)
                    {
                        pointerRenderer.enabled = false;
                    }
                    if (pointerAPIndic.enabled)
                    {
                        pointerAPIndic.enabled = false;
                    }
                    if (pointerAPLeftIndic.enabled)
                    {
                        pointerAPLeftIndic.enabled = false;

                    }
                }
                 
            }
            if(blockMovement1Frame){

                blockMovement1Frame = false;
            
            }
            
            else if (Input.GetMouseButtonUp(0) && Vector3.Distance(spawnedForms[curForm].transform.position, lineEndPos) > 2 && path.corners.Length > 0)
            {
                //Debug.Log("click to move");
                AudioManager.PlayClip(playerWalkSound);
                curAgent.Resume();
                curAgent.SetDestination(lineEndPos);
                curAnimator.SetBool("move", true);
                pointerLine.gameObject.SetActive(false);
                pointerAPIndic.gameObject.SetActive(false);
                pointerAPLeftIndic.gameObject.SetActive(false);
                pointer.gameObject.SetActive(false);

                if (curForm != 1)
                {
                    actionPointsLeft -= pointerDistance + 1;
                }

                pointerDistance = 0;
                state = formState.move;
                
                camFocus.TargetToFollow = spawnedForms[curForm].transform;
            }
        }
        else {
            if (CONTROLS_IS_ON)
            {
                //Debug.Log("no more points left");
                gamemaster.endTurnBtnClicked();
            }
            else
            {
                resetPointer();
            }
        }
    }

    void move()
    {
        if (!CONTROLS_IS_ON)
        {
            actionPointsLeft += (Mathf.FloorToInt(curAgent.remainingDistance) / moveSpeed) + 1;

            curAgent.SetDestination(curAgent.transform.position);
            curAgent.Stop();
            curAnimator.SetBool("move", false);
            camFocus.TargetToFollow = null;
            resetPointer();
            state = formState.idle;
        }
        else
        {
            if (curAgent.remainingDistance < 0.1F)
            {
                curAgent.SetDestination(curAgent.transform.position);
                curAgent.Stop();
                curAnimator.SetBool("move", false);
                camFocus.TargetToFollow = null;
                resetPointer();
                state = formState.idle;
            }

            if (Vector3.Distance(curFormLoc, spawnedForms[curForm].transform.position) < 1)
            {
                if (curUnstuckWait <= 0)
                {
                    curUnstuckWait = unstuckWait;
                    curAgent.SetDestination(curAgent.transform.position);
                    curAgent.Stop();
                    curAnimator.SetBool("move", false);
                    camFocus.TargetToFollow = null;
                    resetPointer();
                    state = formState.idle;
                }
                else
                {
                    curUnstuckWait--;
                }
            }
            else
            {
                if (curUnstuckWait != unstuckWait)
                {
                    curUnstuckWait = unstuckWait;
                }
                curFormLoc = spawnedForms[curForm].transform.position;
            }
        }
    }

    public void resetTurn()
    {
        actionPointsLeft = maxActionPoints;
        pointerLine.gameObject.SetActive(true);
        pointerAPIndic.gameObject.SetActive(true);
        pointerAPLeftIndic.gameObject.SetActive(true);
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

        if (curForm == 0)
        {
            if (!pointerAPIndic.gameObject.activeSelf)
            {
                pointerAPIndic.gameObject.SetActive(true);
                pointerAPIndic.fillAmount = 0;
            }
            if (!pointerAPLeftIndic.gameObject.activeSelf)
            {
                pointerAPLeftIndic.gameObject.SetActive(true);
            }
        }
        else
        {
            if (pointerAPIndic.gameObject.activeSelf)
            {
                pointerAPIndic.gameObject.SetActive(false);
                pointerAPIndic.fillAmount = 0;
            }
            if (pointerAPLeftIndic.gameObject.activeSelf)
            {
                pointerAPLeftIndic.gameObject.SetActive(false);
            }
        }
    }

    void formBtnClicked(int formID)
    {
        if (state != formState.move)
        {
            blockMovement1Frame = true;
            //Debug.Log("UI BUTTON CLICKED");
            if (gamemaster.curTurn == GameMaster.turns.player)
            {
                state = formState.idle;

                if (spawnedForms.Length > formID && spawnedForms[formID] != null)
                {
                    curForm = formID;
                    curAgent = spawnedForms[curForm].GetComponent<NavMeshAgent>();
                    curAnimator = spawnedForms[curForm].GetComponent<Animator>();
                    pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);

                    AudioManager.PlayClip(AudioManager.Instance.mouseclick);
                    if (formID == 0)
                    {
                        pointerLine.GetComponent<LineRenderer>().material.color = physLineColor;
                        camFocus.GoTo("PhysicalForm");
                        //Debug.Log("show real world*******************************************************");
                        gamemaster.ui.Flash.FlashIt(Color.white);
                        //textureHiderManager.ShowPhysicalWorld();
                        Camera.main.GetComponent<CameraShaderManager>().RemoveDigital();
                    }

                    if (formID == 1)
                    {
                        pointerLine.GetComponent<LineRenderer>().material.color = digiLineColor;
                        camFocus.GoTo("DigitalForm(Clone)");
                        //Debug.Log("show internet world*****************************************************");
                        gamemaster.ui.Flash.FlashIt(Color.blue);
                        Camera.main.GetComponent<CameraShaderManager>().SetDigital();

                        //textureHiderManager.HidePhysicalWorld();
                    }

                }
                else if (!isSplitted)
                {
                    if (formID == 1)
                    {
                        TryToSpawnDigital();
                    }
                }
            }
            AudioManager.HandleBackgroundMusic();
        }
    }

    void combineFormsBtnClicked()
    {
        blockMovement1Frame = true;
        if (isSplitted)
        {
            bool allIsClose = true;

            foreach (GameObject form in spawnedForms)
            {
                if (Vector3.Distance(form.transform.position, spawnedForms[0].transform.position) > 8)
                {
                    allIsClose = false;
                    break;
                }
            }

            if (allIsClose)
            {
                //Instantiate(splitExplosion, spawnedForms[0].transform.position, Quaternion.identity);
                //textureHiderManager.ShowPhysicalWorld();

                Camera.main.GetComponent<CameraShaderManager>().RemoveDigital();
                AudioManager.PlayClip(digitalSplitSound);
                curForm = 0;
                curAgent = spawnedForms[curForm].GetComponent<NavMeshAgent>();
                curAnimator = spawnedForms[curForm].GetComponent<Animator>();
                pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);

                AudioManager.HandleBackgroundMusic();

                for (int i = 1; i < spawnedForms.Length; i++)
                {
                    //Instantiate(splitExplosion, spawnedForms[i].transform.position, Quaternion.identity);
                    Destroy(spawnedForms[i]);
                    spawnedForms[i] = null;
                }

                isSplitted = false;
                actionPointsLeft = -1;
                formBtnClicked(0);
                
            }
            else
            {
                //Debug.Log("all forms are not close to each other!");
            }
        }
        else
        {
            //Debug.Log("you need to split before you can combine!");
        }
    }

    void TryToSpawnDigital()
    {
        //try to spawm
        ElectronicUnit[] terminals = GameObject.FindObjectsOfType<ElectronicUnit>();

        //Debug.Log("found: "+terminals.Length);
        bool isTerminalClose = false;
        int terminalID = -1;

        for (int i = 0; i < terminals.Length; i++)
        {
            //Debug.Log("dist: " + Vector3.Distance(spawnedForms[0].transform.position, terminals[i].transform.position) + " usable: " + terminals[i].isUsable);
            if (Vector3.Distance(spawnedForms[0].transform.position, terminals[i].transform.position) < 4 && terminals[i].isUsable)
            {
                //Debug.Log("can split");
                isTerminalClose = true;
                terminalID = i;
            }
        }

        if (isTerminalClose)
        {
            AudioManager.PlayClip(digitalSplitSound);
            //Instantiate(splitExplosion, spawnedForms[curForm].transform.position, Quaternion.identity);
            //Instantiate(splitExplosion, terminals[terminalID].getSpawnPoint(), Quaternion.identity);
            curAnimator.SetTrigger("split");
            //Debug.Log("EXPLODEEEEEE");

            spawnedForms[1] = Instantiate(spawnableForms[1], terminals[terminalID].getSpawnPoint(), Quaternion.identity) as GameObject;
            AudioManager.HandleBackgroundMusic();
            curForm = 1;
            curAgent = spawnedForms[curForm].GetComponent<NavMeshAgent>();
            curAnimator = spawnedForms[curForm].GetComponent<Animator>();
            pointerLine.SetPosition(0, spawnedForms[curForm].transform.position);
            //textureHiderManager.HidePhysicalWorld();
            Camera.main.GetComponent<CameraShaderManager>().SetDigital();
            
            isSplitted = true;
            pointerLine.GetComponent<LineRenderer>().material.color = digiLineColor;
            camFocus.GoTo("DigitalForm(Clone)");
            actionPointsLeft = -1;
            gamemaster.endTurnBtnClicked();
        }
        else
        {
            //Debug.Log("cannot split now into terminal form");
        }
    }

}
