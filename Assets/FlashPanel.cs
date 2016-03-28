using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashPanel : MonoBehaviour {

    public float speed;

    Image img;

	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        img.enabled = false;
        
	}



    public void FlashIt(Color c)
    { 
        StartCoroutine(FlashTransition(c));
    }

    IEnumerator FlashTransition(Color c) {

        img.enabled = true;
        img.color = new Color(c.r, c.g, c.b, 1f) ;
        
        do
        {
            img.color = new Color(c.r,c.g, c.b, img.color.a - 0.05f * speed);
            
            yield return new WaitForEndOfFrame();
        }
        while(img.color.a > 0.1f);

        img.color = new Color(0f, 0f, 0f, 0f);

        img.enabled = false;
        
    }
}
