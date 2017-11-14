using UnityEngine;

/*
 * This script gives the player the ability to use interactable objects. 
 * Interactables are objects which use the interface Interactable.cs 
 * The player uses a ray cast to detect interactables infront of him.
 * OnInteract method of the Interactable will be called if the player interacts 
 * with it.
 */

public class DetectionComponent : PlayerComponentInterface
{
    [Header("General Settings")]
    [Tooltip("The key which triggers detection.")]
    public string detectionButton = "A";
    private int detectionKey;
    [Tooltip("Range of detection.")]
    public float detectionRange = 2;

    // Private Settings
    private GameObject interactableObject;

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        // Set id
        id = 2;

        detectionKey = InputManager.Instance.GetKey(detectionButton, MainPlayer.GetPlayerIndex());
    }

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        GetInteractableObject();
        
        if (interactableObject)
        {
            if (MainPlayer.buttonDownList[detectionKey])
            {
                // Interact with the interactable object
                interactableObject.GetComponent<Interactable>().OnInteract(gameObject);
            }
        }
    }

    private void GetInteractableObject()
    {
        RaycastHit hit;
        Vector3 fwd = gameObject.transform.TransformDirection(Vector3.forward);
        // Debug ray cast
        Debug.DrawRay(transform.position, fwd * detectionRange, Color.green);

        if (Physics.Raycast(transform.position, fwd, out hit, detectionRange))
        {
            Interactable component = hit.transform.gameObject.GetComponent<Interactable>();

            // If component exist, save the interactable object, else delete it
            if (component)
            {
                interactableObject = hit.transform.gameObject;
            }
            else
            {
                interactableObject = null;
            }
        }
        else
        {
            interactableObject = null;
        }
    }
}
