using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorSingle : Interactable {

    private SlidingDoor slidingDoor;

    private void Awake()
    {
        slidingDoor = GetComponentInParent<SlidingDoor>();
    }

    public override void OnInteract(GameObject obj)
    {
        base.OnInteract(obj);

        slidingDoor.OnInteract(obj);
    }

    public override void TurnOnOutline()
    {
        slidingDoor.ChangeOutlineSlidingDoor(false);
    }

    public override void TurnOffOutline()
    {
        slidingDoor.ChangeOutlineSlidingDoor(true);
    }
}
