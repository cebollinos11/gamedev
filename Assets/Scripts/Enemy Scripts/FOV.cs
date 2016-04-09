using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FOV : MonoBehaviour {

    int fieldOfViewRange, visionRange, spotRange;
 
    //raycasting (computical fov)
    List<RaycastHit> hits = new List<RaycastHit>();

    int rayCount;
    float curAngle;
    Vector3 direction;
    // /raycasting
 

    //meshing (visual fov)
    Vector3[] myVertices;
    Vector2[] myUV;
    int[] myTriangles;
    Mesh myMesh;
    MeshRenderer myRenderer;
    Material myMaterial;
    int i;
    int v;
    // /meshing

    //alertStates
    [SerializeField] Color alertNormal, alertSuspect, alertKill;
    public enum alertState
    {
        normal,
        investigate,
        kill
    }
    public alertState alert = alertState.normal;
    //

    void Start()
    {
        myMesh = GetComponent<MeshFilter>().mesh;
        myRenderer = GetComponent<MeshRenderer>();
        myMaterial = myRenderer.material;

        myMaterial.color = alertNormal;
    }

    public void init(int fovRange, int visDist, int spotDist = 0)
    {
        fieldOfViewRange = fovRange;
        visionRange = visDist;
        spotRange = spotDist;
    }

    public void genRays()
    {
        rayCount = fieldOfViewRange / 2; //how many raycasts to generate. I set it to half of the field of view (set higher for lower quality but higher performance)

        //curAngle is used for which angle to cast current raycast
        curAngle = fieldOfViewRange / -2;

        hits.Clear(); //clear last hits

        for (int r = 0; r < rayCount; r++)
        {
            direction = Quaternion.AngleAxis(curAngle, transform.up) * transform.forward; //direction to cast ray
            RaycastHit hit = new RaycastHit();

            //if no hit
            if(Physics.Raycast(transform.position, direction, out hit, visionRange) == false)
            {
                hit.point = transform.position + (direction * visionRange); //just make the point at the end of vision range
            }

            hits.Add(hit); //add to list of points

            curAngle += fieldOfViewRange/rayCount;
        }
    }

    public void genMesh()
    {
        if(hits != null && hits.Count > 0) //if we have generated the hits
        {
            //if number of vertices vs number of hit points doesnt match
            if(myMesh.vertices.Length != hits.Count + 1)
            {
                myMesh.Clear(); //destry the mesh
                myVertices = new Vector3[hits.Count + 1]; //make the vertices array as big as the number of hit points
                myTriangles = new int[(hits.Count - 1) * 3]; //make the triangles array as big as the number of hit points times 3 (because there is 3 vertices in a triangle)

                i = 0;
                v = 1;
                while(i < myTriangles.Length)
                {
                    if((i % 3) == 0) //makes sure we have 3 unused places
                    {
                        myTriangles[i] = 0;
                        myTriangles[i + 1] = v;
                        myTriangles[i + 2] = v + 1;
                        v++;
                    }
                    i++;
                }
            }

            myVertices[0] = Vector3.zero; //first vertex is my pos
            for (i = 1; i <= hits.Count; i++)
            {
                //convert the hit point to local position of the mesh and add as vertices
                myVertices[i] = transform.InverseTransformPoint(hits[i - 1].point);
            }

            //UV map for texture
            myUV = new Vector2[myVertices.Length];
            i = 0;
            while(i < myUV.Length)
            {
                myUV[i] = new Vector2(myVertices[i].x, myVertices[i].z);
                i++;
            }

            //generate the FOV mesh
            myMesh.vertices = myVertices;
            myMesh.triangles = myTriangles;
            myMesh.uv = myUV;

            myMesh.RecalculateNormals();
            myMesh.RecalculateBounds();

            checkAlertState();
        }
    }

    public void checkAlertState()
    {
        switch(alert)
        {
            case alertState.normal:
                if (myMaterial.color != alertNormal) myMaterial.color = alertNormal;
                break;
            case alertState.investigate:
                if (myMaterial.color != alertSuspect) myMaterial.color = alertSuspect;
                break;
            case alertState.kill:
                if (myMaterial.color != alertKill) myMaterial.color = alertKill;
                break;
            default:
                if (myMaterial.color != alertNormal) myMaterial.color = alertNormal;
                break;
        }
    }

    public void genFOV()
    {
        genRays();
        genMesh();
    }

    public void clearMesh()
    {
        myMesh.Clear();
    }

    public int canSeePlayer()
    {
        // 0 = cant see him
        // 1 = player spotted
        // 2 = investigate
        // 3 = opticform
        int state = 0;

        foreach(RaycastHit hit in hits)
        {
            if(hit.transform && hit.transform.tag == "Player")
            {
                if(Vector3.Distance(hit.transform.position, transform.parent.position) <= spotRange)
                {
                    state = 1;
                    alert = alertState.kill;
                }
                else
                {
                    state = 2;
                    alert = alertState.investigate;
                }
                break;
            }
            else if (hit.transform && hit.transform.tag == "OpticForm")
            {
                state = 3;
                alert = alertState.investigate;
            }
        }

        return state;
    }
}
