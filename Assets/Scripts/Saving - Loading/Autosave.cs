using UnityEngine;

// Autosave when entering scene. Add always on the beginning, but NOT with players inside the collider. 

public class Autosave : MonoBehaviour{

    bool saved;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!saved)
            {
                SaveLoadControl.Instance.SaveData(false);
                saved = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
