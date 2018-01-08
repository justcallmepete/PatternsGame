using UnityEngine;
using EazyTools.SoundManager;

/*
 * Elevator script opens the elevator if the player has a keycard. 
 * Use on elivator object. 
 */ 

public class Elevator : Interactable {

    private Animator anim;
    private bool locked = true;

    public AudioClip elevatorDoorOpenSFX;

	// Use this for initialization
	public override void Start () { 
        base.Start();
        anim = transform.parent.gameObject.GetComponent<Animator>();
	}

    public override void OnInteract(GameObject obj)
    {
        if (obj.GetComponent<MainPlayer>().inventory.Keycard)
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
        SoundManager.PlaySound(elevatorDoorOpenSFX, 0.1f);
    }

    public void UnlockElevator()
    {
        locked = false;
    }
}
