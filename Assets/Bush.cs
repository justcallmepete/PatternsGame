using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour {
    [Tooltip("Set alpha when in normal state.")]
    public float alphaNormal;
    [Tooltip("Set alpha when in hiding state.")]
    public float alphaTransparent;
    [Tooltip("Set minimal tiling.")]
    public float minTiling = 0.5f;
    [Tooltip("Set maximal tiling.")]
    public float maxTiling = 1.5f;

    private float alphaTarget;
    private Material[] materials;
    private float value = 0.0f;
    const float lerpSpeed = 0.3f;

	void Start () {
        SetRandomTextureScale();
        alphaTarget = alphaNormal;
    }

    private void SetRandomTextureScale()
    {
        materials = gameObject.GetComponent<MeshRenderer>().materials;
        float scaleX = Random.Range(minTiling, maxTiling);
        float scaleY = Random.Range(minTiling, maxTiling);
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].mainTextureScale = new Vector2(scaleX, scaleY);
        }
    }

    public void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Player")
        {
            SetAlphaTarget(alphaTransparent);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SetAlphaTarget(alphaNormal);
        }
    }

    private void Update()
    {
        SetObjectAlpha(alphaTarget);
    }

    private void SetObjectAlpha(float alpha)
    {
        if (value > 1f)
        {
            return; 
        }

        for (int i = 0; i < materials.Length; i++)
        {
            Color color = materials[i].color;
            color.a = Mathf.Lerp(color.a, alphaTarget, value);
            materials[i].color = color;
        }

        value += lerpSpeed * Time.deltaTime;
    }

    private void SetAlphaTarget(float alpha)
    {
        alphaTarget = alpha;
        value = 0.0f;
    }
}
