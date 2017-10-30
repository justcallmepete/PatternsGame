using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : PlayerComponent  {	
	// Update is called once per frame
	public override void UpdateComponent ()
    {
        CheckButton();
        CheckButtonDown();
        CheckAxisDirection();
	}

    private void CheckButton()
    {
        for (int i = 0; i < MainPlayer.buttonList.Length; i++)
        {
            if (Input.GetButton(MainPlayer.playerIndex + "_Button_" + i))
            {
                MainPlayer.buttonList[i] = true;
            }
            else
            {
                MainPlayer.buttonList[i] = false;
            }
        }
    }

    private void CheckButtonDown()
    {
        for (int i = 0; i < MainPlayer.buttonDownList.Length; i++)
        {
            if (Input.GetButtonDown(MainPlayer.playerIndex + "_Button_" + i))
            {
                MainPlayer.buttonDownList[i] = true;
                Debug.Log(MainPlayer.playerIndex + "_Button_" + i);
            }
            else
            {
                MainPlayer.buttonDownList[i] = false;
            }
        }
    }

    // Get players movement input
    private void CheckAxisDirection()
    {
        float axis_Horizontal = Input.GetAxis(MainPlayer.playerIndex + "_Axis_1");
        float axis_Vertical = Input.GetAxis(MainPlayer.playerIndex + "_Axis_2");
        Vector3 axisDir = new Vector3(axis_Horizontal, 0, axis_Vertical);
        MainPlayer.axisDirection = Quaternion.Euler(0, 45, 0) * axisDir;
    }
}
