using UnityEngine;
using cakeslice;
// Interface class is used to initialize interactable methods. Implement when you have a interactable object.
[RequireComponent(typeof(Outline))]

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector]
    public Outline outline;

    public virtual void Start()
    {
        outline = gameObject.GetComponent<Outline>();

        if (!outline)
        {
            gameObject.AddComponent<Outline>();
            outline = gameObject.GetComponent<Outline>();
        }
    }

    public virtual void OnInteract(GameObject obj) { }
    public virtual void ChangeOutline(bool eraseRenderer)
    {
        print(outline);
        outline.eraseRenderer = eraseRenderer;
    }

    public virtual void TurnOffOutline()
    {
        ChangeOutline(true);
    }

    public virtual void TurnOnOutline()
    {
        ChangeOutline(false);
    }
}
