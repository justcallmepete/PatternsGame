using UnityEngine;

/*
 * Keycard script to put it in the inventory of the player.
 */ 

public class Keycard : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<MainPlayer>().inventory.Keycard = true;
            Destroy(gameObject);
        }
    }
}
