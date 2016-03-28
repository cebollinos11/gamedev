using UnityEngine;
using System.Collections;

public class TextureHiderManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


    public void HidePhysicalWorld() {

        TextureHider[] realwalls = Object.FindObjectsOfType<TextureHider>();

        for (int i = 0; i < realwalls.Length; i++)
        {
            realwalls[i].Hide();
        }
    
    }

    public void ShowPhysicalWorld()
    {

        TextureHider[] realwalls = Object.FindObjectsOfType<TextureHider>();

        for (int i = 0; i < realwalls.Length; i++)
        {
            realwalls[i].Show();
        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            HidePhysicalWorld();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowPhysicalWorld();
        }
    }
}
