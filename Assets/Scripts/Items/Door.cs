using System.Collections;
using UnityEngine;

/*
 * Door script changes the state of the door. The rotation of the door is animated by code. 
 * The door rotation is fully adjustable in the inspector.
 * The method TryMove can be called to change the state of the door. This method must be called by the player.
 * The methods Open en Close can be use to force the door to open or close. These methods must be called by events.
 */

public class Door : Interactable
{
    // Inspector Settings
    [Header("Rotation Settings")]
    [Tooltip("The initial angle of the door/window.")]
    public float initialAngle = 0.0F;
    [Tooltip("The amount of degrees the door/window rotates.")]
    public float rotationAngle = 90.0F;
    public enum SideOfRotation { Left, Right }
    public SideOfRotation rotationSide;
    [Tooltip("Rotating speed of the door/window.")]
    public float speed = 10F;
    [Tooltip("0 = infinite times")]
    public int timesMoveable = 0;
    [Tooltip("Boolean if the door is locked with a key")]
    public bool lockedWithKey = false;

    public enum TypeOfHinge { Centered, CorrectlyPositioned }
    [Header("Hinge Settings")]
    public TypeOfHinge hingeType;

    public enum PositionOfHinge { Left, Right }
    public PositionOfHinge hingePosition;
    
    // Private Settings
    private int timesRotated = 0;
    private bool lockedWithSwitch = false;
    public bool LockedWithSwitch { get { return lockedWithSwitch; } set { lockedWithSwitch = value; } }
    [HideInInspector] public bool rotationPending = false; // Needs to be public because Detection.cs has to access it

    // Debug Settings
    [Header("Debug Settings")]
    [Tooltip("Visualizes the position of the hinge in-game by a colored cube.")]
    public bool visualizeHinge = false;
    [Tooltip("The color of the visualization of the hinge.")]
    public Color hingeColor = Color.yellow;

    // Define an initial and final rotation
    Quaternion finalRot, initialRot;
    public enum State 
    {
        Open,
        Closed,
    };

    private State currentState = State.Open;
    public State CurrentState { get { return currentState; } }

    private Coroutine lastCoroutine;

    // Create a hinge
    GameObject hinge;

    // An offset to take into account the original rotation of a 3rd party door
    Quaternion rotationOffset;

    void Start()
    {
        // Give the object the name "Door" for future reference
        gameObject.tag = "Door";

        rotationOffset = transform.rotation;

        if (hingeType == TypeOfHinge.Centered)
        {
            // Create a hinge
            hinge = new GameObject("hinge");

            // Calculate sine/cosine of initial angle (needed for hinge positioning)
            float cosDeg = Mathf.Cos((transform.eulerAngles.y * Mathf.PI) / 180);
            float sinDeg = Mathf.Sin((transform.eulerAngles.y * Mathf.PI) / 180);

            // Read transform (position/rotation/scale) of the door
            Vector3 positionDoor = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 rotationDoor = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            Vector3 scaleDoor = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

            // Create a placeholder/temporary object of the hinge's position/rotation
            Vector3 hingePosCopy = hinge.transform.position;
            hingePosCopy.y = positionDoor.y;
            Vector3 hingeRotCopy = new Vector3(rotationDoor.x, -initialAngle, rotationDoor.z);

            if (hingePosition == PositionOfHinge.Left)
            {
                if (transform.localScale.x > transform.localScale.z)
                {
                    hingePosCopy.x = positionDoor.x - (scaleDoor.x / 2 * cosDeg);
                    hingePosCopy.z = positionDoor.z + (scaleDoor.x / 2 * sinDeg);
                }
                else
                {
                    hingePosCopy.x = positionDoor.x + (scaleDoor.z / 2 * sinDeg);
                    hingePosCopy.z = positionDoor.z + (scaleDoor.z / 2 * cosDeg);
                }
            }

            if (hingePosition == PositionOfHinge.Right)
            {
                if (transform.localScale.x > transform.localScale.z)
                {
                    hingePosCopy.x = positionDoor.x + (scaleDoor.x / 2 * cosDeg);
                    hingePosCopy.z = positionDoor.z - (scaleDoor.x / 2 * sinDeg);
                }
                else
                {
                    hingePosCopy.x = positionDoor.x - (scaleDoor.z / 2 * sinDeg);
                    hingePosCopy.z = positionDoor.z - (scaleDoor.z / 2 * cosDeg);
                }
            }

            // Hinge Positioning
            hinge.transform.position = hingePosCopy;
            transform.parent = hinge.transform; 
            hinge.transform.localEulerAngles = hingeRotCopy;

            // Debugging
            if (visualizeHinge == true)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = hingePosCopy;
                cube.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                cube.GetComponent<Renderer>().material.color = hingeColor;
            }
        }
    }

    public override void OnInteract(GameObject obj)
    {
        if (lockedWithSwitch)
        {
            //Debug.Log("Door is locked by a switch");
            return;
        }

        if (lockedWithKey)
        {
            //Debug.Log("Door is locked");
            // TODO Check if the player has a key
        }


        switch (currentState)
        {
            case State.Closed:
                Open();
                break;
            case State.Open:
                Close();
                break;
        }
    }

    public void Open()
    {
        StopLastCoroutine();
        lastCoroutine = StartCoroutine(DoOpen());
    }

    public void Close()
    {
        StopLastCoroutine();
        lastCoroutine = StartCoroutine(DoClose());
    }

    public void StopLastCoroutine()
    {
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
    }

    // Move Function
    public IEnumerator DoOpen()
    {
        currentState = State.Open;
        // Angles
        if (rotationSide == SideOfRotation.Left)
        {
            finalRot = Quaternion.Euler(0, -initialAngle - rotationAngle, 0);
        }

        if (rotationSide == SideOfRotation.Right)
        {
            finalRot = Quaternion.Euler(0, -initialAngle + rotationAngle, 0);
        }

        if (timesRotated < timesMoveable || timesMoveable == 0)
        {
            if (hingeType == TypeOfHinge.Centered)
            {           
                // Set 'FinalRotation' to 'FinalRot' when moving
                Quaternion FinalRotation = finalRot;

                // Make the door/window rotate until it is fully opened
                while (Mathf.Abs(Quaternion.Angle(FinalRotation, hinge.transform.rotation)) > 0.01f)
                {
                    rotationPending = true;
                    hinge.transform.rotation = Quaternion.Lerp(hinge.transform.rotation, FinalRotation, Time.deltaTime * speed);
                    yield return new WaitForEndOfFrame();
                }
                rotationPending = false;
            }
            else
            {
                // Set 'FinalRotation' to 'FinalRot' when moving
                Quaternion FinalRotation = finalRot * rotationOffset;

                // Make the door/window rotate until it is fully opened
                while (Mathf.Abs(Quaternion.Angle(FinalRotation, transform.rotation)) > 0.01f)
                {
                    rotationPending = true;
                    transform.rotation = Quaternion.Lerp(transform.rotation, FinalRotation, Time.deltaTime * speed);
                    yield return new WaitForEndOfFrame();
                }
                rotationPending = false;
            }

            if (timesMoveable == 0)
            {
                timesRotated = 0;
            }
            else
            {
                timesRotated++;
            }
        }
    }

    // Move Function
    public IEnumerator DoClose()
    {
        currentState = State.Closed;
        // Angles
        if (rotationSide == SideOfRotation.Left)
        {
            initialRot = Quaternion.Euler(0, -initialAngle, 0);
        }

        if (rotationSide == SideOfRotation.Right)
        {
            initialRot = Quaternion.Euler(0, -initialAngle, 0);
        }

        if (timesRotated < timesMoveable || timesMoveable == 0)
        {
            if (hingeType == TypeOfHinge.Centered)
            {            
                // Set 'FinalRotation' to 'InitialRot' when moving back
                Quaternion FinalRotation = initialRot;

                // Make the door/window rotate until it is fully closed
                while (Mathf.Abs(Quaternion.Angle(FinalRotation, hinge.transform.rotation)) > 0.01f)
                {
                    rotationPending = true;
                    hinge.transform.rotation = Quaternion.Lerp(hinge.transform.rotation, FinalRotation, Time.deltaTime * speed);
                    yield return new WaitForEndOfFrame();
                }
                rotationPending = false;
            }
            else
            {       
                // Set 'FinalRotation' to 'InitialRot' when moving back
                Quaternion FinalRotation = initialRot * rotationOffset;

                // Make the door/window rotate until it is fully closed
                while (Mathf.Abs(Quaternion.Angle(FinalRotation, transform.rotation)) > 0.01f)
                {
                    rotationPending = true;
                    transform.rotation = Quaternion.Lerp(transform.rotation, FinalRotation, Time.deltaTime * speed);
                    yield return new WaitForEndOfFrame();
                }
                rotationPending = false;
            }

            if (timesMoveable == 0)
            {
                timesRotated = 0;
            }
            else
            {
                timesRotated++;
            }
        }
    }
}
