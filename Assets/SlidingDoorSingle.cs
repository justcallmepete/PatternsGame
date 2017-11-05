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
}
