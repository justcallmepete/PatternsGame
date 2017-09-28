using UnityEngine;

//this script controls the movement of both players, it uses acceleration, deceleration and rotation
public class Controlable : MonoBehaviour
{

    public enum PlayerIndex
    {
        Player1, Player2
    }

    public PlayerIndex playerIndex;
    public int sprintButton;
    public int sneakButton;

    [Range(100, 200)]
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
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

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
    void FixedUpdate()
    {
        //Set max speed and rotation speed to sprint speed or walk speed
        if (Input.GetAxis("P" + playerNumber + "_Button_" + sprintButton) != 0)
        {
            maxSpeed = sprintSpeed;
            rotationSpeed = rotationSprintSpeed;
        }
        else if (Input.GetAxis("P" + playerNumber + "_Button_" + sneakButton) != 0)
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
        if (GetAxisDirection() != Vector3.zero)
        {
            saveDirection = GetAxisDirection();
        }

        // Rotate our transform a step closer to the target's
        Vector3 targetRotation = Vector3.Normalize(GetAxisDirection());
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
        // Apply movement
        Move();
    }

    private void Move()
    {
        //Apply speed
        if (GetAxisDirection() != Vector3.zero)
        {
            rb.velocity = speed * GetAxisDirection() * Time.deltaTime;
        }
        else
        {
            rb.velocity = speed * saveDirection * Time.deltaTime;
        }
    }

    private void CheckAcceleration()
    {
        //Apply accelararion
        if (GetAxisDirection() != Vector3.zero)
        {
            if (speed < maxSpeed)
            {
                speed += accelaration;
            }
        }
    }

    private void CheckDeceleration()
    {
        //if the player is moving faster than the max speed, decelerate
        if (GetAxisDirection() != Vector3.zero)
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

    // Get players movement input
    private Vector3 GetAxisDirection()
    {
        float axis_Horizontal = Input.GetAxis("P" + playerNumber + "_Axis_1");
        float axis_Vertical = Input.GetAxis("P" + playerNumber + "_Axis_2");
        Vector3 axisDir = new Vector3(axis_Horizontal, 0, axis_Vertical);
        return axisDir;
    }
}
