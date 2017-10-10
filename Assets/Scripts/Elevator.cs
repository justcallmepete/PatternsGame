using UnityEngine;

/*
 * Elevator script opens the elevator if the player has a keycard.
 */ 

public class Elevator : MonoBehaviour {

    private Animator anim;
    private bool locked = true;

	// Use this for initialization
	void Start () {
        anim = transform.parent.gameObject.GetComponent<Animator>();
	}

    public void TryOpen()
    {
        if (locked)
        {
            Debug.Log("Elevator is Locked");
            return;
        }

        Open();
    }

    public void Open()
    {
        anim.SetBool("doOpen", true);
    }

    public void UnlockElevator()
    {
        locked = false;
    }
}
