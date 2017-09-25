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

    private Material[] mat;
	// Use this for initialization
	void Start () {
        mat = gameObject.GetComponent<MeshRenderer>().materials;
        float scaleX = Random.Range(0.5f, 1.5f);
        float scaleY = Random.Range(0.5f, 1.5f);
        mat[0].mainTextureScale = new Vector2(scaleX, scaleY);
    }

    public void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Player")
        {
            SetObjectAlpha(alphaTransparent);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SetObjectAlpha(alphaNormal);
        }
    }

    // Change transparency
    private void SetObjectAlpha(float alpha)
    {
        for (int i = 0; i < mat.Length; i++)
        {
            Color color = mat[i].color;
            color.a = alpha;
            mat[i].color = color;
            Debug.Log(color.a);
        }
    }
}
