using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This console calls the ConsoleComponent.onConsole() when the user interacts with the console.
 */

public class Console : Interactable
{
    public List<ConsoleComponent> linkedObjects;

    public override void OnInteract(GameObject obj)
    {
        base.OnInteract(obj);
        Debug.Log("OnInteract");
        for (int i = 0; i < linkedObjects.Count; i++)
        {
            linkedObjects[i].onConsole();
        }
    }
}
