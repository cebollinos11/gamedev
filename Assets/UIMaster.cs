using UnityEngine;
using System.Collections;

public class UIMaster : MonoBehaviour {


    public GameObject GameOver;
    public GameObject Wait;
    public FlashPanel Flash;

	// Use this for initialization
	void Start () {
        GameOver.SetActive(false);
        Wait.SetActive(false);
        SetCursor();
        
        
	}

    void SetCursor()
    {
        Texture2D cursorTexture = Resources.Load("Cursor") as Texture2D;
        Cursor.SetCursor(cursorTexture, Vector2.zero,CursorMode.Auto);

    }

    

    public void ShowGameOver() {
        GameOver.SetActive(true);
    
    }

    public void ShowWait() {
        Wait.SetActive(true);
    }

    public void HideWait() {
        Wait.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
