using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * This components should be added to an object that will be linked to a console. 
 * In the inspector you can link the method that should be called when the console is activated.
 */
public class ConsoleComponent : MonoBehaviour {
    public UnityEvent methods;

    public void onConsole()
    {
        Debug.Log("methods.Invoke()");
        methods.Invoke();
    }
}
