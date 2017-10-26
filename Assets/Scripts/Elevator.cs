using UnityEngine;

/*
 * Elevator script opens the elevator if the player has a keycard.
 */ 

public class Elevator : Interactable {

    private Animator anim;
    private bool locked = true;

	// Use this for initialization
	void Start () {
        anim = transform.parent.gameObject.GetComponent<Animator>();
	}

    public override void OnInteract(GameObject obj)
    {
        if (obj.GetComponent<Inventory>().Keycard)
        {
            UnlockElevator();
            Open();

            return;
        }
        Debug.Log("Elevator is Locked");
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
