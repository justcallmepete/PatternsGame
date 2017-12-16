using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCamera : MonoBehaviour {
    public List<MainPlayer> playersInVision;
    public bool playerInLight { get{ return playersInVision.Count > 0; } }
    bool isAlerted;

    [SerializeField]
    Color colorStandard, colorMid, colorAlert;

    Material visionMaterial;
    GameObject guardCameraVision;

    enum RotateState { rotatingToTarget, reachedTarget, waitingForNextTarget, none };
    RotateState currentState;
    enum RotateMode { loop, backAndForth, random }
    [SerializeField]
    RotateMode myRotateMode;
    //Rotate parameters
    [SerializeField]
    float rotateSpeed;
    [SerializeField, Range(0,360)]
    float[] watchAngles;
    Vector3[] watchDirections;
    [SerializeField]
    float rotateDelay;

    int targetDirection;
    int previousTarget;

    [ExecuteInEditMode]
    void OnValidate()
    {
        if (watchAngles.Length <= 0) watchAngles = new float[1];
        watchAngles[0] = 0;
    }

    void Start () {
        guardCameraVision = transform.Find("GuardCameraVisualisation").gameObject;
        playersInVision = new List<MainPlayer>();
        visionMaterial = guardCameraVision.GetComponentInChildren<Renderer>().material;
        isAlerted = false;

        watchDirections = new Vector3[watchAngles.Length];
        for (int i = 0; i < watchDirections.Length; i++)
        {
            //get dirction
            watchDirections[i] = Mainframe.utils.MathUtils.DirFromAngle(watchAngles[i]+ transform.eulerAngles.y, true, this.transform);
            Debug.Log(transform.position + watchDirections[i]);
            Debug.DrawLine(transform.position, transform.position + watchDirections[i] * 50,Color.white, 10f);
        }
        targetDirection = 0;

        GetNextRotatePosition();
	}
	
	// Update is called once per frame
	void Update () {

        switch (currentState)
        {
            case RotateState.reachedTarget:
                GetNextRotatePosition();
                break;
            case RotateState.waitingForNextTarget:
                //Handle waiting time
                break;
            case RotateState.rotatingToTarget:
                //Handle rotation
                HandleRotate();
                break;
        }

        if (isAlerted)
        {
            //TO DO: Check if no players in sight   
            if(playersInVision.Count <= 0)
            {
                SetSemiAlert();
            }
            return;
        }

        if (playerInLight)
        {
            Alert();
        }
	}

    void HandleRotate()
    {
        // distance between target and the actual rotating object
        Vector3 dir = transform.position + watchDirections[targetDirection] - transform.position;

        // calculate the Quaternion for the rotation
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);

        //Apply the rotation 
        transform.rotation = rotation;

        // put 0 on the axys you do not want for the rotation object to rotate
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


        if(Vector3.Angle(transform.forward, watchDirections[targetDirection]) < 1f)
        {
            //Reached target
            ReachedTarget();
        }
    }

    void GetNextRotatePosition()
    {
        targetDirection += 1;
        int nextDirection = targetDirection;

        switch (myRotateMode)
        {
            case RotateMode.loop:
                if(targetDirection > watchDirections.Length - 1)
                {
                    targetDirection = 0;
                    nextDirection = 0;
                }
                break;
            case RotateMode.backAndForth:
                if(previousTarget >= targetDirection) //is moving back
                {
                    if(previousTarget > 2)
                    {
                        nextDirection -= 2;
                    }
                }
                else if(targetDirection > watchDirections.Length - 1) //reached end, start moveback
                {
                    nextDirection = watchDirections.Length - 2;
                }
                break;
            case RotateMode.random:
                nextDirection = Random.Range(0, watchDirections.Length - 1);
                break;
        }

        RotateToPosition(nextDirection);
    }

    void RotateToPosition(int pPosition)
    {
        previousTarget = targetDirection;
        targetDirection = pPosition;
        currentState = RotateState.rotatingToTarget;
    }

    void ReachedTarget()
    {
        Debug.Log("Reached target");
        currentState = RotateState.reachedTarget;
    }

    void Alert()
    {
        isAlerted = true;
        SetVisionColor(colorAlert);
    }

    void SetSemiAlert()
    {
        isAlerted = false;
        SetVisionColor(colorMid);
    }

    void setStandard()
    {
        SetVisionColor(colorStandard);
    }

    void SetVisionColor(Color pColor)
    {
        visionMaterial.SetColor("_Color", pColor);
    }
}
