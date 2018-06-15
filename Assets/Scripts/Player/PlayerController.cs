using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Command buttonW, buttonS, buttonShift, nothing;

    public GameObject PlayerGameObject;
    private MovementComponent PMC;
    private TeleportComponent PTC;

    private void Start ()
    {
        // Get components for moving and teleporting
        PMC = PlayerGameObject.GetComponent<MovementComponent>();
        PTC = PlayerGameObject.GetComponent<TeleportComponent>();
        // bind Command to Class
		buttonW = new MoveForward(PMC);
        buttonS = new MoveBackwards(PMC);
        buttonShift = new Teleport(PTC);
        nothing = new DoNothing(PMC);
	}

    // Update is called once per frame
   private void Update()
    {
        HandleInput();
    }

    // Handle input of button, Execute command attached to the button
   void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            buttonW.Execute();
        }

        if (Input.GetKey(KeyCode.S))
        {
            buttonS.Execute();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            buttonShift.Execute();
        }

        // added a nothing for default movement state (reset)
        nothing.Execute();
    }
}
