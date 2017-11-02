using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : Interactable
{
    float timer;
    public float openTime = 3;

    private bool lockedWithSwitch = false;
    public bool LockedWithSwitch { get { return lockedWithSwitch; } set { lockedWithSwitch = value; } }
    private Animator animator;

    public enum State
    {
        Open,
        Closed,
    };
    private State currentState = State.Closed;

    // Use this for initialization
    void Start () {
        animator = GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animator.GetBool("open")&&!lockedWithSwitch)
        {
            timer += Time.deltaTime;
            if(timer >= openTime)
            {
                animator.SetBool("open", false);
                timer = 0f; 
            }
        } 

	}

    public override void OnInteract(GameObject obj)
    {
        print("testing");
        if (lockedWithSwitch)
        {
            Debug.Log("Door is locked by a switch");
            return;
        }

        switch (currentState)
        {
            case State.Closed:
                Open();
                break;
            case State.Open:
                Close();
                break;
        }
    }
    public void Open()
    {
        //open animation
        Debug.Log("open");
        currentState = State.Open;
        animator.SetBool("open", true);
    }

    public void Close()
    {
        //close animation
        Debug.Log("close");
        currentState = State.Closed;
        animator.SetBool("open", false);
    }
}
