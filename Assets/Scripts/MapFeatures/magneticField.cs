using UnityEngine;
using System.Collections;

public class magneticField : MonoBehaviour {


    bool active;
 
    SphereCollider spCollider;
    TextureTiler tT;
    FormsManager fM;

	// Use this for initialization
	void Start () {
        active = true;
        tT = GetComponent<TextureTiler>();
        spCollider = GetComponent<SphereCollider>();
        fM = GameObject.FindObjectOfType<FormsManager>();

        StartCoroutine(ListenToSplits());
   
	
	}

    IEnumerator ListenToSplits() {

        while (true)
        {

            yield return new WaitForSeconds(1.0f);
            if (fM.isSplitted && active)
                TurnOff();
            if (!fM.isSplitted && !active)
                TurnOn();
        }

    }

    public void TurnOff() {
        active = false;

        spCollider.enabled = false;
        tT.speed /= 10f;

        Debug.Log("turning off");
    }

    public void TurnOn() {
        active = true;
        spCollider.enabled = true;
        tT.speed *= 10f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {

            TurnOff();
            
        }
	}
}
