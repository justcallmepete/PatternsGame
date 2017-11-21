using UnityEngine;

// Autosave when entering scene. Add always on the beginning, but NOT with players inside the collider. 

public class Autosave : MonoBehaviour{



    private void OnTriggerEnter(Collider other)
    {        
         if (other.gameObject.tag == "Player")
         {
             
             SaveLoadControl.Instance.SaveData(false);
        
         }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
