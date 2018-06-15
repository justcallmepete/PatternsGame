using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

   // public static List<Command> OldCommands = new List<Command>();
    private Command buttonW, buttonS, buttonShift, nothing;

    public GameObject PlayerGameObject;
    private MovementComponent PMC;
    private TeleportComponent PTC;


    // Use this for initialization
    private void Start ()
    {
        PMC = PlayerGameObject.GetComponent<MovementComponent>();
        PTC = PlayerGameObject.GetComponent<TeleportComponent>();
	//	buttonW = new MoveForward(PMC);
        buttonS = new MoveBackwards(PMC);
        buttonShift = new Teleport(PTC);
        buttonW = new Teleport(PTC);
        nothing = new DoNothing(PMC);
	}

    // Update is called once per frame
   private void Update()
    {
        HandleInput();
    }

   void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("buttonW executed");
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

        nothing.Execute();
    }
}
