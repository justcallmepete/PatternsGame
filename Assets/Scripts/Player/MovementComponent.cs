using UnityEngine;

public class MovementComponent : PlayerComponent
{
    public int sprintButton = 1;
    public int sneakButton = 0;

    [Header("Movement variables")]
    [Tooltip("Normal movement speed")]
    [Range(100, 200)]
    public float movementSpeed;
    [Tooltip("Maximal sprint speed")]
    [Range(200, 500)]
    public float sprintSpeed;
    [Tooltip("Minimal sneak speed")]
    [Range(50, 150)]
    public float sneakSpeed;

    [Tooltip("Normal rotation speed")]
    [Range(10, 30)]
    public float rotationMovementSpeed;
    [Tooltip("Sprint rotation speed")]
    [Range(10, 30)]
    public float rotationSprintSpeed;

    [Tooltip("Acceleration of the player")]
    [Range(10, 30)]
    public float acceleration;
    [Tooltip("Deceleration of the player")]
    [Range(10, 30)]
    public float deceleration;
    [Tooltip("Full stop deceleration of the player")]
    [Range(10, 50)]
    public float fullStopDeceleration;

    [Range(10, 40)]
    public float fullStopSensitivity;

    private float rotationSpeed;
    private float speed;
    private float maxSpeed;
    private Vector3 saveDirection;

    public override void AwakeComponent()
    {
        base.AwakeComponent();

    }

    public override void FixedUpdateComponent()
    {
        base.FixedUpdateComponent();

        SetMovementValues();

    }

    private void SetMovementValues()
    {
        if (!MainPlayer.IsBusy())
        {

            //Set max speed and rotation speed to sprint speed or walk speed
            if (MainPlayer.buttonList[sprintButton])
            {
                maxSpeed = sprintSpeed;
                rotationSpeed = rotationSprintSpeed;
            }
            else if (MainPlayer.buttonList[sneakButton])
            {
                maxSpeed = sneakSpeed;
                rotationSpeed = rotationMovementSpeed;
            }
            else
            {
                maxSpeed = movementSpeed;
                rotationSpeed = rotationMovementSpeed;
            }

            // Save the direction when the direction is not zero so the controller knows which direction to decelerate in
            if (MainPlayer.axisDirection != Vector3.zero)
            {
                saveDirection = MainPlayer.axisDirection;
            }
        }
        // Rotate our transform a step closer to the target's
        Vector3 targetRotation = Vector3.Normalize(MainPlayer.axisDirection);
        if (targetRotation != Vector3.zero)
        {
            //calculate difference between the current rotation and the target rotation in angles
            float rotationDifference = Mathf.Abs(Quaternion.LookRotation(targetRotation).eulerAngles.y - transform.rotation.eulerAngles.y);

            //rotate transform 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetRotation), rotationSpeed);

            //check the difference in angle to do a full stop
            if (rotationDifference > 180 - fullStopSensitivity && rotationDifference < 180 + fullStopSensitivity)
            {
                FullStop();
            }
        }

        // Apply aceleration when needed
        CheckAcceleration();
        //apply deceleration when needed
        CheckDeceleration();
        // Apply movement when the player is free
        if (MainPlayer.IsFree())
        {
            Move();
        }
    }

    private void Move()
    {
        //Apply speed
        if (MainPlayer.axisDirection != Vector3.zero)
        {
            MainPlayer.rigidBody.velocity = speed * MainPlayer.axisDirection * Time.deltaTime;
        }
        else
        {
            MainPlayer.rigidBody.velocity = speed * saveDirection * Time.deltaTime;
        }
    }

    private void FullStop()
    {
        while (speed > 0)
        {
            speed -= fullStopDeceleration;
            if (speed <= 0)
            {
                speed = 0;
            }
        }
    }
    private void CheckAcceleration()
    {
        //Apply accelararion
        if (MainPlayer.axisDirection != Vector3.zero)
        {
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
        }
    }

    private void CheckDeceleration()
    {
        //if the player is moving faster than the max speed, decelerate
        if (MainPlayer.axisDirection != Vector3.zero)
        {
            if (speed > maxSpeed)
            {
                speed -= deceleration;
                if (speed < 0) speed = 0;
            }
        }
        //if the player is not moving but is not stationary, decelerate
        else
        {
            if (speed > 0)
            {
                speed -= deceleration;
                if (speed < 0) speed = 0;
            }
        }
    }
}
