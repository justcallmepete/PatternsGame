using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour {

    private bool isHiding = false;
    private MeshRenderer mesh;

	// Use this for initialization
	void Start () {
        // Get meshrenderer
        mesh = gameObject.GetComponentInChildren<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hideable")
        {
            // Become invisible when hiding
            isHiding = true;
            SetObjectAlpha(mesh.materials, 0.2f);        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hideable")
        {
            // Become visibile when hiding
            isHiding = false;
            SetObjectAlpha(mesh.materials);
        }
    }

    // Change transparency
    private void SetObjectAlpha(Material[] mat, float alpha = 1f)
    {
        for (int i = 0; i < mat.Length; i++)
        {
            Color color = mat[i].color;
            color.a = alpha;
            mat[i].color = color;
        }
    }

    // Getter for isHiding
    public bool IsHiding { get { return isHiding; } }
}
