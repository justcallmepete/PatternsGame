using UnityEngine;

/*
 * Controls button and axis input and updates it in the main player class.
 */ 

public class InputComponent : PlayerComponentInterface
{
    public override void AwakeComponent()
    {
        base.AwakeComponent();

        // Set id
        id = 0;
    }

    public override void UpdateComponent ()
    {
        base.UpdateComponent();

        CheckButton();
        CheckAxisDirection();
	}

    private void CheckButton()
    {
        for (int i = 0; i < MainPlayer.buttonList.Length; i++)
        {
            // Check when button is pressed 
            if (Input.GetButton(MainPlayer.playerIndex + "_Button_" + i))
            {
                MainPlayer.buttonList[i] = true;
            }
            else
            {
                MainPlayer.buttonList[i] = false;
            }

            // Check when button is pressed down
            if (Input.GetButtonDown(MainPlayer.playerIndex + "_Button_" + i))
            {
                MainPlayer.buttonDownList[i] = true;
            }
            else
            {
                MainPlayer.buttonDownList[i] = false;
            }
        }
    }

    // Get axis input
    private void CheckAxisDirection()
    {
        float axis_Horizontal = Input.GetAxis(MainPlayer.playerIndex + "_Axis_1");
        float axis_Vertical = Input.GetAxis(MainPlayer.playerIndex + "_Axis_2");
        Vector3 axisDirection = new Vector3(axis_Horizontal, 0, axis_Vertical);
        MainPlayer.axisDirection = Quaternion.Euler(0, 45, 0) * axisDirection;
    }
}
