using UnityEngine;
using System.Collections;

public class ElectronicUnit : MonoBehaviour {

    public bool isUsable = true;
    Transform player;
    public TextMesh text;

    FormsManager fm;

    [SerializeField] 

    Transform spawnpoint;
	// Use this for initialization
	void Start () {
        spawnpoint = transform.FindChild("spawnpoint");
        player = GameObject.Find("PhysicalForm").transform;
        fm = GameObject.FindObjectOfType<FormsManager>();
	}

    public Vector3 getSpawnPoint()
    {
        return spawnpoint.position;
    }
   
    IEnumerator OpenText() {

        

        float t = 0f;
        do
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime*10;
            text.transform.localScale = new Vector3(t, 1, 1);

        } while (t < 1);

        text.transform.localScale = new Vector3(1, 1, 1);
    
    }

    void Update() {


        if (Vector3.Distance(fm.spawnedForms[0].transform.position, transform.position) < 4 && isUsable)
        {
            if (fm.isSplitted == false)
            {

                if (text.text != "Split allowed...")
                {
                    text.transform.localScale = Vector3.zero;
                    StartCoroutine(OpenText());
                    Debug.Log(text.text);
                }

                text.text = "Split allowed...";
                text.color = Color.blue;
            }

            else
            {
                if (Vector3.Distance(fm.spawnedForms[0].transform.position, fm.spawnedForms[1].transform.position) < 8)
                {

                    

                    if (text.text != "Merge allowed...")
                    {
                        text.transform.localScale = Vector3.zero;
                        StartCoroutine(OpenText());
                        Debug.Log(text.text);
                    }
                    text.text = "Merge allowed...";
                    text.color = Color.cyan;
                }
                else
                {
                   
                    text.text = "";
                }
            }
        }

        else {
            text.text = "";
        }
    
    }
}
