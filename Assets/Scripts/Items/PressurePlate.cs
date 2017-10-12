using UnityEngine;

/*
 * This script interacts with game objects which have a mass. 
 * A minimal amount of mass is need to open the locked door.
 */

public class PressurePlate : MonoBehaviour {
    [Header("General Settings")]
    [Tooltip("A locked object which must be opened by using this pressure plate.")]
    public GameObject lockedObject;
    [Tooltip("Player must keep standing on this pressure plate to unlock.")]
    public bool mustHold = false;
    [Tooltip("Minimal amount of mass needed to press the plate.")]
    public float minimalMass = 1;

    // Private settings
    private float currentMass = 0;
    private Door door;

    // Use this for initialization
    void Start()
    {
        if (!lockedObject)
        {
            Debug.LogWarning("No locked object assigned.");
            return;
        }

        // Lock the door
        door = lockedObject.GetComponent<Door>();
        door.LockedWithSwitch = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        // Stop when no locked object is defined
        if (!lockedObject)
        {
            return;
        }

        // Stop when no there is no rigidbody
        if (!other.gameObject.GetComponent<Rigidbody>())
        {
            return;
        }

        // Add mass 
        currentMass += other.gameObject.GetComponent<Rigidbody>().mass;

        // Check mass difference
        if (currentMass >= minimalMass)
        {
            Debug.Log("Open the door");
            door.Open();

            // Remove the lock if must not hold
            if (!mustHold)
            {
                door.LockedWithSwitch = false;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Stop if the player doesn't have to hold the button
        if (!mustHold)
        {
            return;
        }

        // Stop when no locked object is defined
        if (!lockedObject)
        {
            return;
        }

        // Stop when no there is no rigidbody
        if (!other.gameObject.GetComponent<Rigidbody>())
        {
            return;
        }

        // Remove mass
        currentMass -= other.gameObject.GetComponent<Rigidbody>().mass;  

        // Check mass difference
        if (currentMass < minimalMass)
        {
            Debug.Log("Lock the door");
            door.Close();
        }
    }
}
