using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlable : MonoBehaviour {

    public enum PlayerIndex
    {
        Player1, Player2 
    }

    public PlayerIndex playerIndex;
    public int sprintButton;
    public int sneakButton;

    [Range(100,200)]
    public float movementSpeed;
    [Range(200, 500)]
    public float sprintSpeed;
    [Range(50, 150)]
    public float sneakSpeed;

    [Range(10, 30)]
    public float rotationMovementSpeed;
    [Range(10, 30)]
    public float rotationSprintSpeed;

    [Range(10, 30)]
    public float accelaration;
    [Range(10, 30)]
    public float deceleration;
    [Range(10, 50)]
    public float fullStopDeceleration;

    [Range(10, 40)]
    public float fullStopSensitivity;

    private float rotationSpeed;
    private float speed;
    private float maxSpeed;
    private Vector3 saveDirection;
    private int playerNumber;
    private bool decelerateBool = false;

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
	void FixedUpdate () {

        //Set max speed and rotation speed to sprint speed or walk speed
        if (Input.GetAxis("P" + playerNumber + "_Button_" + sprintButton) !=0)
        {
            maxSpeed = sprintSpeed;
            rotationSpeed = rotationSprintSpeed;
        }
        else if(Input.GetAxis("P" + playerNumber + "_Button_" + sneakButton) !=0)
        {
            maxSpeed = sneakSpeed;
            if (speed >sneakSpeed && GetMovementAxis() != Vector3.zero)
            {
                speed -= deceleration;
                if (speed < 0) speed = 0;
            }
        }
        else
        {
            maxSpeed = movementSpeed;
            rotationSpeed = rotationMovementSpeed;
            if (speed>movementSpeed&&GetMovementAxis()!=Vector3.zero)
            {
                speed -= deceleration;
                if (speed < 0) speed = 0;
            }
        }
        //Apply accelararion
        if (speed < maxSpeed && GetMovementAxis() != Vector3.zero && decelerateBool == false)
        {
            speed += accelaration;
        }
        //save the direction when the direction is not zero so the controller knows which direction to decelerate in
        if (GetMovementAxis() != Vector3.zero)
        {
            saveDirection = GetMovementAxis();
        }
        //apply decelaration
        if (GetMovementAxis() == Vector3.zero&& speed > 0)
        {
            speed -= deceleration;
            if (speed < 0) speed = 0;    
        }

        // Rotate our transform a step closer to the target's
        Vector3 targetRotation = Vector3.Normalize(getRotationAxis());
        if (targetRotation != Vector3.zero)
        {
            //calculate difference between the current rotation and the target rotation in angles
            float rotationDifference = Mathf.Abs( Quaternion.LookRotation(targetRotation).eulerAngles.y - transform.rotation.eulerAngles.y);
            
            //rotate transform 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetRotation), rotationSpeed);

            if (rotationDifference > 180-fullStopSensitivity&& rotationDifference< 180+fullStopSensitivity)
            {
                FullStop();
            }
        }

        //apply movement
        Move();
        
    }
    private void FullStop()
    {
        decelerateBool = true;

        while(speed>0)
        {
            speed -= fullStopDeceleration;
            if (speed <= 0)
            {
                speed = 0;
                decelerateBool = false;
            }
        }
    }
    private void Move()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        //Apply speed
        if (decelerateBool==false)
        {
            if (GetMovementAxis() != Vector3.zero)
            {
                rb.velocity = speed * GetMovementAxis() * Time.deltaTime;
            }
            else
            {
                rb.velocity = speed * saveDirection * Time.deltaTime;
            }
        }
    }

    private Vector3 GetMovementAxis()
    {
        float axis_Horizontal = Input.GetAxis("P" + playerNumber + "_Axis_1");
        float axis_Vertical = Input.GetAxis("P" + playerNumber + "_Axis_2");
        Vector3 axisDir = new Vector3(axis_Horizontal, 0, axis_Vertical);
        return axisDir;
    }

    public Vector3 getRotationAxis()
    {
        float axis_Horizontal = Input.GetAxis("P" + playerNumber + "_Axis_1");
        float axis_Vertical = Input.GetAxis("P" + playerNumber + "_Axis_2");
        Vector3 axisdir = new Vector3(axis_Horizontal, 0, axis_Vertical);
        return axisdir;
    }
}
