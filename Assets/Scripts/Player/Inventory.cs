using UnityEngine;

public class Inventory : MonoBehaviour{
    private bool keycard = false;
    public bool Keycard { get { return keycard; } set { keycard = value; Debug.Log("keycard");  } }
}
