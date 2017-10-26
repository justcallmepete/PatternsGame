using UnityEngine;
using System.Linq;

/*
 * This script detects objects with certain tags and interacts with those stuff.
 */

public class Detection : MonoBehaviour
{
    // General Settings
    [Header("General Settings")]
    public int key = 1;
    public float detectionRange = 2;
    // Private Settings
    private Inventory inventory;
    private string[] tags = new string[] { "Door", "Elevator" };
    private GameObject obj;
    // Controls of the player
    private Controlable controlable;

    void Start()
    {
        inventory = gameObject.GetComponentInParent<Inventory>();
        controlable = gameObject.GetComponentInParent<Controlable>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            obj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            // Remove object when it exits the collider
            obj = null;
    }

    void Update()
    {
        if (obj && obj.GetComponent<Interactable>())
        {
            
            if (controlable.GetButtonDown(key))
            {
                obj.GetComponent<Interactable>().OnInteract(gameObject);
            }
        }
        GetInteractableObject();
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
