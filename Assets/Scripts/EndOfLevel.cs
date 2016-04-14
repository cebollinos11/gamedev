using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {
    FormsManager fm;
    public string NextLevelName;
    bool triggeredAlready;
    void Start() {

        fm = GameObject.FindObjectOfType<FormsManager>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!fm.isSplitted) 
        {
            if (!triggeredAlready) {
                triggeredAlready = true;
                Debug.Log("SHOW LEVEL COMPLETE!");
                Debug.Log(Application.loadedLevel);
                Invoke("GoToNextLevel", 2f);

            
            }
        }
        
    }

    void GoToNextLevel()
    {

        Debug.Log("GO TO NEXT LEVEL");
        Application.LoadLevel(Application.loadedLevel+1);

    }
}
