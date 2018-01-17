using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : Interactable {
    private Elevator elevator;

    private void Awake()
    {
        elevator = GetComponentInParent<Elevator>();
    }

    public override void OnInteract(GameObject obj)
    {
        base.OnInteract(obj);

        elevator.OnInteract(obj);
    }

    public override void TurnOnOutline()
    {
        elevator.ChangeOutlineSlidingDoor(false);
    }

    public override void TurnOffOutline()
    {
        elevator.ChangeOutlineSlidingDoor(true);
    }
}

