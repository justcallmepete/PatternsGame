using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class opens and closes pop up menu

public class MenuManager : MonoBehaviour {

    public GameObject popUpMenu;
    bool menuIsOpen;
    GameObject instance;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (menuIsOpen)
            {
                menuIsOpen = false;
                Destroy(instance);
            }

            else if (!menuIsOpen)
            {
                menuIsOpen = true;
                instance = Instantiate(popUpMenu);
            }
        }
    }
}
