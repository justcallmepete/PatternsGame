
using UnityEngine;

// Triggers the save function for a checkpoint

public class Checkpoint : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {


            if (other.gameObject.tag == "Player")
            {
                SaveLoadControl.Instance.SaveData(true);
            }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position , transform.localScale);
    }
}
