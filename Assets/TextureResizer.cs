using UnityEngine;
using System.Collections;

public class TextureResizer : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
         float multiplier = 0.5f;
        //GetComponent<MeshRenderer>().material = material;
        Vector2 scale = new Vector2(transform.lossyScale.x, transform.lossyScale.z) * multiplier;
        GetComponent<MeshRenderer>().material.mainTextureScale = scale;
        GetComponent<MeshRenderer>().material.SetTextureScale("_BumpMap", scale);
	
	}
	
	
}
