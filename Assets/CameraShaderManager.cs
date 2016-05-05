using UnityEngine;
using System.Collections;

public class CameraShaderManager : MonoBehaviour {

    public Material MaskShader;
    public Material WaterShader;
    public Material NegativeShader;
    public Material DigitalShader;

    Material currentMaterial;

    public AnimationCurve Sin;

    // Use this for initialization
    void Start()
    {

        
        RunStart();
        //currentMaterial = DigitalShader;
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

    public void SetDigital() {

        currentMaterial = DigitalShader;

    }

    public void RemoveDigital() {

        currentMaterial = null;
    }

    public void SetOptic()
    {

        currentMaterial = NegativeShader;
    }

    public void RemoveOptic()
    {
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
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            RunDeath();

        }


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
