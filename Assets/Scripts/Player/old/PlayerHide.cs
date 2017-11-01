/* 
    This script changes the player transparency when it collides with an object with a tag "Hideable"
    The boolean if it's hiding can be called with IsHiding.
 */
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    private MeshRenderer mesh;
    private bool isHiding = false;

    public bool IsHiding { get { return isHiding; } }

    // Use this for initialization
    void Start()
    {
        // Get meshrenderer
        mesh = gameObject.GetComponentInChildren<MeshRenderer>();
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
}
