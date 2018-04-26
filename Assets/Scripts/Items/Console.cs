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
        for (int i = 0; i < linkedObjects.Count; i++)
        {
            if (linkedObjects[i] == null) return;
            linkedObjects[i].OnConsole();
        }
    }
}
