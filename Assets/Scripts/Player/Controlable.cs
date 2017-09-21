using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlable : MonoBehaviour {

    public enum PlayerIndex
    {
        Player1, Player2 
    }

    public PlayerIndex playerIndex;
    public float movementSpeed;
    public float sprintSpeed;
    public int sprintButton;


    private float speed;
    private int playerNumber;


	// Use this for initialization
	void Start () {
        switch (playerIndex) {
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
        Rigidbody rb = this.GetComponent<Rigidbody>();
        if (Input.GetAxis("P" + playerNumber + "_Button_" + sprintButton) !=0)
        {
            speed = sprintSpeed;
        } else
        {
            speed = movementSpeed;
        }

        rb.velocity = speed * GetMovementAxis() * Time.deltaTime;
	}

    private Vector3 GetMovementAxis()
    {
        float axis_Horizontal = Input.GetAxis("P" + playerNumber + "_Axis_1");
        float axis_Vertical = Input.GetAxis("P" + playerNumber + "_Axis_2");
        Vector3 axisDir = new Vector3(axis_Horizontal, 0, axis_Vertical);
        return axisDir;
    }

    //public Vector3 GetRotationAxis()
    //{
    //    float axis_Horizontal = Input.GetAxis("P" + playerNumber + "_Axis_4");
    //    float axis_Vertical = Input.GetAxis("P" + playerNumber + "_Axis_5");
    //    Vector3 axisDir = new Vector3(axis_Horizontal, 0, axis_Vertical);
    //    return axisDir;
    //}
}
