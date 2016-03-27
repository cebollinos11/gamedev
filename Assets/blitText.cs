using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class blitText : MonoBehaviour {

    Text T;

    public float speed;
    float c;

	// Use this for initialization
	void Start () {
	    T = GetComponent<Text>();
        c=1;
	}
	
	// Update is called once per frame
	void Update () {

        c-=Time.deltaTime*speed;

        if(c<0)
        {
            T.enabled = !T.enabled;
            c = 1;

        }

        
	
	}
}
