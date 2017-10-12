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
        if (obj)
        {
            if (controlable.GetButtonDown(key))
            {
                switch (obj.tag)
                {
                    case "Elevator":
                        OpenElevator();
                        break;
                    case "Door": obj.GetComponent<Door>().TryMove();
                        break;
                    default: break;

                }
                

            }
        }
    }

   private void OpenElevator()
    {
        Elevator elevator = obj.GetComponentInParent<Elevator>();

        // Use keycard
        if (inventory.Keycard)
        {
            elevator.UnlockElevator();
            inventory.Keycard = false;
        }

        elevator.TryOpen();
    }         
}
