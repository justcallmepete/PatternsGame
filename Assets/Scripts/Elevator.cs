using UnityEditor;
using UnityEngine;

/*
 * Elevator script opens the elevator if the player has a keycard. 
 * Use on elivator object. 
 */ 

public class Elevator : Interactable {

    private Animator anim;
    private bool locked = true;

    public bool isExitElevator = true;
    GameObject[] players;
    Vector3[] playerPositions;
    GameObject floor;


    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerPositions = new Vector3[2];
        floor = GameObject.FindGameObjectWithTag("Floor");
        floor.GetComponent < MeshCollider>().enabled = false;
        for (int i = 0; i < players.Length; i++)
        {
            playerPositions[i] = players[i].transform.position;
            players[i].transform.position = new Vector3(playerPositions[i].x, playerPositions[i].y - 10, playerPositions[i].z);
        }
        anim = gameObject.GetComponent<Animator>();

        if (isExitElevator)
        {
            anim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath("Assets/Animations/Elevator_depart.controller", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        else
        {
            anim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath("Assets/Animations/Elevator_arrive.controller", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
	}

    public override void OnInteract(GameObject obj)
    {
        if (obj.GetComponent<MainPlayer>().inventory.Keycard)
        {
            UnlockElevator();
            Open();

            return;
        }
        Debug.Log("Elevator is Locked");
    }

    public void Open()
    {
        anim.SetBool("doOpen", true);
    }

    public void UnlockElevator()
    {
        locked = false;
    }

    public void ElevatorArrived()
    {       
        floor.GetComponent<MeshCollider>().enabled = true;
    }
}
