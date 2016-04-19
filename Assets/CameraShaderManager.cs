using UnityEngine;
using System.Collections;

public class CameraShaderManager : MonoBehaviour {

    public Material MaskShader;

    Material currentMaterial;

    public AnimationCurve Sin;

    // Use this for initialization
    void Start()
    {
       // RunDeath();
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
