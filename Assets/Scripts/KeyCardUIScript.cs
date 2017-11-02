using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *
 *  This class is used on the keycardUI. It is placed on a empty gameobject above the sprite. 
 *  Choose player 1 or 2 in the inspector.
 * 
 */

public class KeyCardUIScript : MonoBehaviour {

    public enum PlayerIndex
    {
        P1, P2
    }

    [Header("UI setting for player")]
    public PlayerIndex playerIndex;

    GameObject[] players;
    GameObject player;
    [SerializeField]
    GameObject keycardSprite;
    bool playerIsFound = false;

    // Use this for initialization
    void Start() {

        players = GameObject.FindGameObjectsWithTag("Player");


        int i = 0;
        do
        {
            string playerName = players[i].GetComponent<MainPlayer>().getPlayerIndex();
            if(playerName == playerIndex.ToString())
            {
                playerIsFound = true;
                player = players[i];
            }
            
            i++;

        } while (!playerIsFound || i < players.Length);

        if (!playerIsFound || i > players.Length)
        {
            Debug.Log("Cannot find player!");
        }
        
	}

	void Update () {
        if (player == null)
        {
            Debug.LogError("KeyCard player: " + playerIndex + " is not found");
            return;
        }
        // Set the UI true, 
        if (player.GetComponent<Inventory>().Keycard == true)
        {
            keycardSprite.SetActive(true);
        }
        else
        {
            keycardSprite.SetActive(false);
        }
		
	}
}
