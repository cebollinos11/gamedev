using UnityEngine;
using System.Collections;

public class CameraShaderManager : MonoBehaviour {

    public Material MaskShader;
    public Material ShaderWin;
    public Material WaterShader;
    public Material NegativeShader;
    public Material DigitalShader;

    Material currentMaterial;

    public AnimationCurve Sin;

    public bool isOptic;

    // Use this for initialization
    void Start()
    {

        
        RunStart();
        //RunWin();
        //currentMaterial = DigitalShader;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            RunWin();
        }

    }

    IEnumerator StartAnimation()
    {

        currentMaterial = WaterShader;

        float c = 0.02f;

        do
        {
            c -= Time.deltaTime/100;
            float val = Sin.Evaluate(c);
            currentMaterial.SetFloat("_WaterAmplitude", val);
            yield return new WaitForEndOfFrame();
        } while (c>0.0f);

        currentMaterial = null;
    }

    IEnumerator DeathAnimation()
    {

        currentMaterial = MaskShader;

        float c = 0.0f;

        do
        {
            c += Time.deltaTime;
            float val = Sin.Evaluate(c);
            currentMaterial.SetFloat("_MaskAmount", val);
            yield return new WaitForEndOfFrame();
        } while (true);


    }

    IEnumerator WinAnimation()
    {

        //GetComponent<CameraFocuser>().FocusOnCurrentForm();
        currentMaterial = ShaderWin;

        
        float t = 1f;

        do
        {
            t -= Time.deltaTime;
            transform.Rotate(new Vector3(Time.deltaTime * 20f,0.0f,0.0f));
            //transform.position = (transform.position + new Vector3(0.0f, -Time.deltaTime * 20f, 0.0f));
            
            yield return new WaitForEndOfFrame();
        } while (t>0f);

        t = 1f;

        do
        {
           t-=Time.deltaTime;
            //transform.Rotate(new Vector3(Time.deltaTime * 20f,0.0f,0.0f));
            transform.position = (transform.position + new Vector3(0.0f, -Time.deltaTime * 20f, 0.0f));
            yield return new WaitForEndOfFrame();
        } while (t>0f);


    }

    public void SetDigital() {

        currentMaterial = DigitalShader;

    }

    public void RemoveDigital() {

        currentMaterial = null;
    }

    public void SetOptic()
    {
        isOptic = true;
        currentMaterial = NegativeShader;
    }

    public void RemoveOptic()
    {
        isOptic = false;
        currentMaterial = null;
    }

    public void RunStart()
    {
        StartCoroutine(StartAnimation());
    }


    public void RunDeath()
    {
        StartCoroutine(DeathAnimation());
    }

    public void RunWin()
    {
        StartCoroutine(WinAnimation());
    }



    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (currentMaterial == null)
        {
            Graphics.Blit(source, destination);

            return;
        }


        Graphics.Blit(source, destination, currentMaterial);
    }
}
