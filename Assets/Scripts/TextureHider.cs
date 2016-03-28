using UnityEngine;
using System.Collections;

public class TextureHider : MonoBehaviour {


    Material originalMat;
    Renderer rend;

    Material hiddenMat;
    public bool isWire;

	// Use this for initialization
	void Start () {
        originalMat = GetComponent<Renderer>().material;
        rend = GetComponent<Renderer>();

        if (isWire) {
            hiddenMat = Resources.Load("ActiveWire") as Material;
        }

        else
        {
            hiddenMat = Resources.Load("Transparent") as Material;
        }
            
	}

    public void Hide() {
        Debug.Log("hiding");
        //rend.material.SetFloat("_Mode", 3);
        //rend.material.color = Color.black;
        Debug.Log(gameObject);
        rend.material = hiddenMat;
    }

    public void Show()
    {
        Debug.Log("showing");
        rend.material = originalMat;
    }

    


	
}
