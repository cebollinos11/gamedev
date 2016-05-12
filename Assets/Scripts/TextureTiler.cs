using UnityEngine;
using System.Collections;

public class TextureTiler : MonoBehaviour
{
    [SerializeField] bool skinnedMeshRenderer = false;

    public float speed;
    // Use this for initialization

    MeshRenderer mr;
    SkinnedMeshRenderer smr;

    [SerializeField]
    bool otherAxis;

    void Start()
    {
        if (skinnedMeshRenderer)
        {
            smr = GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            mr = GetComponent<MeshRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 v = Vector2.right;
        if(otherAxis)
        {
            v = Vector2.up;
        }
        if (!skinnedMeshRenderer)
        {
            Vector2 currentOffset = mr.material.mainTextureOffset;
            mr.material.mainTextureOffset = currentOffset + v * speed * Time.deltaTime;
        }
        else
        {
            Vector2 currentOffset = smr.material.mainTextureOffset;
            smr.material.mainTextureOffset = currentOffset + v * speed * Time.deltaTime;
        }
    }
}
