using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *  This class contains the logic for the whistle mechanic.
 */
public class Whistle : MonoBehaviour {

    public enum PlayerIndex
    {
        Player1, Player2
    }
    //Easy player selection in the inspector
    public PlayerIndex playerIndex;
    //Button Code for activating the ability.
    public int button;
    //Radius of the soundwave
    public float radius;
    //How long does the sound stay active?
    public float lifeTime;
    //Player 1/2/3/etc
    private int playerNumber;

    private void Start()
    {
        switch (playerIndex)
        {
            case PlayerIndex.Player1:
                playerNumber = 1;
                break;
            case PlayerIndex.Player2:
                playerNumber = 2;
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("P" + playerNumber + "_Button_" + button))
        {
            SoundWave obj = SoundWave.Create(radius, lifeTime);
            obj.transform.position = this.transform.position;
        }
	}
}
