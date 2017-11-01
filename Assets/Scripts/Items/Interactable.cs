
using UnityEngine;


// Interface class is used to initialize interactable methods. Implement when you have a interactable object.

public abstract class Interactable : MonoBehaviour {


     
    public virtual void OnInteract(GameObject obj) { } 
    
}
