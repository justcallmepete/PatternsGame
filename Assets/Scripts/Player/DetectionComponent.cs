using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script gives the player the ability to use interactable objects. 
 * Interactables are objects which use the interface Interactable.cs 
 * The player uses a ray cast to detect interactables infront of him.
 * OnInteract method of the Interactable will be called if the player interacts 
 * with it.
 */

public class DetectionComponent : PlayerComponent
{
    [Header("General Settings")]
    public int key = 1;
    public float detectionRange = 2;
    // Private Settings
    private GameObject obj;

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        GetInteractableObject();

        if (obj && obj.GetComponent<Interactable>())
        {
            if (MainPlayer.buttonDownList[key])
            {
                obj.GetComponent<Interactable>().OnInteract(gameObject);
            }
        }
    }

    private void GetInteractableObject()
    {
        RaycastHit hit;
        Vector3 fwd = gameObject.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * detectionRange, Color.green);
        if (Physics.Raycast(transform.position, fwd, out hit, detectionRange))
        {
            obj = hit.transform.gameObject;
        }
        else
        {
            obj = null;
        }
    }
}
