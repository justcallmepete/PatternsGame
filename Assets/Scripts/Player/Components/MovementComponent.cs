using UnityEngine;

/*
 * Applies movement and rotation to the object
 */ 

public class MovementComponent : PlayerComponentInterface
{
    [Header("Movement variables")]
    [Tooltip("Normal movement speed")]
    [Range(100, 450)]
    public float movementSpeed;

    [Tooltip("Normal rotation speed")]
    [Range(10, 30)]
    public float rotationSpeed;

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

    [Range(0, 1)]
    public float startRunningSpeed;

    private float speed;
    private float maxSpeed;
    private Vector3 saveDirection;

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        id = 1;
    }

    public override void FixedUpdateComponent()
    {
        if (MainPlayer.IsDead())
        {
            return;
        }

        base.FixedUpdateComponent();
        if (MainPlayer.IsBusy())
        {
            return;
        }
       // SetMovementValues();
    }

    public void SetMovementValues()
    {
        if (MainPlayer.IsChannelling())
        {
            speed = 0;
        }
        //Set max speed and rotation speed to sprint speed or walk speed
        if (MainPlayer.axisDirection != Vector3.zero)
        {
            maxSpeed = movementSpeed;
        }
        else
        {
            maxSpeed = 0;
        }

        // Save the direction when the direction is not zero so the controller knows which direction to decelerate in
        if (MainPlayer.axisDirection != Vector3.zero)
        {
            saveDirection = MainPlayer.axisDirection;
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


        Move();

        MainPlayer.animator.SetFloat("inputV", Mathf.Abs(MainPlayer.rigidBody.velocity.x));
        MainPlayer.animator.SetFloat("inputH", Mathf.Abs(MainPlayer.rigidBody.velocity.z));

        if (MainPlayer.axisDirection.magnitude > startRunningSpeed && !MainPlayer.IsChannelling())
        {
            MainPlayer.animator.SetBool("run", true);
        }
        else
        {
            MainPlayer.animator.SetBool("run", false);
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
        if (MainPlayer.IsChannelling()) return;
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
