using UnityEngine;

/*
 * If a door is infront the player, he can open the door by clicking the key.
 */ 

public class Detection : MonoBehaviour
{
    // GENERAL SETTINGS
    [Header("General Settings")]
    public int key = 1;

    private Door door;
    private Controlable controlable;

    void Start()
    {
        controlable = gameObject.GetComponentInParent<Controlable>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Door") {
            if (!door)
            {
                // Get Door component
                door = other.GetComponent<Door>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Door")
        {
            // Set door to null if the door is not in range.
            door = null;
        }
    }

    void Update()
    {
        if (door)
        {
            if (controlable.GetButtonDown(key))
            {
                // Open/close the door by running the 'Open' function found in the 'Door' script
                door.TryMove();

            }
        }
    }

          
}
