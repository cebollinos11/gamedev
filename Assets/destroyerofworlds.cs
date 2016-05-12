using UnityEngine;
using System.Collections;

public class destroyerofworlds : MonoBehaviour {

    [SerializeField]
    Material partyShader;

	// Use this for initialization
	void Start () {
	    Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = partyShader;
        }
	}
	
	
}
