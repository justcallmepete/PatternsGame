/* 
 * The alpha of the bush changes when a player enters the bush. Also each bush has a random tiling,
 * resulting in a different texture each time.
 */

using UnityEngine;

public class Bush : MonoBehaviour
{
    [Tooltip("Set alpha when in normal state.")]
    public float alphaNormal;
    [Tooltip("Set alpha when in hiding state.")]
    public float alphaTransparent;
    [Tooltip("Set minimal tiling.")]
    public float minTiling = 0.5f;
    [Tooltip("Set maximal tiling.")]
    public float maxTiling = 1.5f;

    private Material[] materials;
    // Alpha to lerp to
    private float alphaTarget;
    // Lerp value has a range [0, 1]
    private float lerpValue = 0.0f;
    // Speed to lerp
    const float lerpSpeed = 0.3f;

    void Start()
    {
        SetRandomTextureScale();
        alphaTarget = alphaNormal;
    }

    private void SetRandomTextureScale()
    {
        // Randomize the tiling 
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
            // Change to transparent
            SetAlphaTarget(alphaTransparent);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Reset transparency
            SetAlphaTarget(alphaNormal);
        }
    }

    private void Update()
    {
        SetObjectAlpha(alphaTarget);
    }

    private void SetObjectAlpha(float alpha)
    {
        // Stops lerping when value is greater or equal than 1
        if (lerpValue >= 1f)
        {
            return;
        }

        // Lerp alpha
        for (int i = 0; i < materials.Length; i++)
        {
            Color color = materials[i].color;
            color.a = Mathf.Lerp(color.a, alphaTarget, lerpValue);
            materials[i].color = color;
        }

        // Increase lerp value
        lerpValue += lerpSpeed * Time.deltaTime;
    }

    private void SetAlphaTarget(float alpha)
    {
        alphaTarget = alpha;
        // Reset lerp value
        lerpValue = 0.0f;
    }
}
