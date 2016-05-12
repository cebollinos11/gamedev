using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    GameObject ButtonStart, ButtonExit,Panel;

    void PlaySound()
    {

        Application.LoadLevel(1);
    }

    public void LoadGame()
    {
        //

        ButtonExit.SetActive(false);
        ButtonStart.SetActive(false);
        Panel.SetActive(false);
        GetComponent<AudioSource>().Play();
        Invoke("PlaySound", 4f);
    }
	

}
