﻿using UnityEngine;
using cakeslice;
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
    private Interactable interactableObject;

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        // Set id
        id = 2;

        // InputManager instance accessible from everywhere
        // map key for teleport
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
                MainPlayer.animator.SetBool("interact", true);
                // Interact with the interactable object
                interactableObject.GetComponent<Interactable>().OnInteract(gameObject);
            }
            else
            {
                MainPlayer.animator.SetBool("interact", false);
            }
        }
        else
        {
            MainPlayer.animator.SetBool("interact", false);
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
                SetOutlineRenderer(true);
                interactableObject = hit.transform.gameObject.GetComponent<Interactable>();
                SetOutlineRenderer(false);
            }
            else
            {
                SetOutlineRenderer(true);
                interactableObject = null;
            }
        }
        else
        {
            SetOutlineRenderer(true);
            interactableObject = null;
        }
    }

    private void SetOutlineRenderer(bool eraseRenderer)
    {
        if (interactableObject)
        {
            switch (eraseRenderer)
            {
                case false:
                    interactableObject.TurnOnOutline();
                    break;
                default:
                    interactableObject.TurnOffOutline();
                    break;
            }
        }
    }
}
