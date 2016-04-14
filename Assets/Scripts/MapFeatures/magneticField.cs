using UnityEngine;
using System.Collections;

public class magneticField : MonoBehaviour {


    bool active;
    public GameObject innerSphere;
 
    SphereCollider spCollider;
    TextureTiler tT;
    FormsManager fM;

    public GameObject ParticlesToSpawn;
    public AudioClip ParticleSound;


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

        innerSphere.SetActive(false);

        spCollider.enabled = false;
        tT.speed /= 10f;

        Debug.Log("turning off");
    }

    public void TurnOn() {
        active = true;
        spCollider.enabled = true;
        tT.speed *= 10f;
        innerSphere.SetActive(true);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            fM.gamemaster.ui.Flash.FlashIt(Color.blue);
            AudioManager.PlayClip(ParticleSound);
            GameObject par =  Instantiate(ParticlesToSpawn, fM.curAgent.transform.position, Quaternion.identity) as GameObject;
            par.transform.parent = fM.curAgent.transform;
        }
    
    }
    void OnTriggerStay(Collider col) {

        if (col.gameObject.tag == "Player")
        {
            

            Vector3 vector;

            vector = fM.curAgent.transform.position - transform.position;
            vector = Vector3.Scale(vector, new Vector3(1f, 0f, 1f));
            vector = vector.normalized;

            fM.curAgent.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

            fM.curAgent.SetDestination(fM.curAgent.transform.position+vector*2f);
            

            
        }
            
        
    }
	
	
}
