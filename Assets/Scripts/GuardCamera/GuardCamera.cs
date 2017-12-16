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

    //Rotate parameters
    enum RotateState { rotatingToTarget, reachedTarget, waitingForNextTarget, none };
    RotateState currentState;
    enum RotateMode { loop, backAndForth, random }
    [SerializeField]
    RotateMode myRotateMode;
    [SerializeField]
    float rotateSpeed;
    [SerializeField, Range(0,360)]
    float[] watchAngles;
    Vector3[] watchDirections;
    [SerializeField]
    float rotateDelay;

    int rotateTargetDirection;
    int rotatePreviousTarget;

    [SerializeField]
    GuardStateMachine[] guardsToAlert;

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
        rotateTargetDirection = 0;

        setStandard();
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
            else
            {
                AlertGuards();
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
        Vector3 dir = transform.position + watchDirections[rotateTargetDirection] - transform.position;

        // calculate the Quaternion for the rotation
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);

        //Apply the rotation 
        transform.rotation = rotation;

        // put 0 on the axys you do not want for the rotation object to rotate
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


        if(Vector3.Angle(transform.forward, watchDirections[rotateTargetDirection]) < 1f)
        {
            //Reached target
            ReachedTarget();
        }
    }

    void GetNextRotatePosition()
    {
        rotateTargetDirection += 1;
        int nextDirection = rotateTargetDirection;

        switch (myRotateMode)
        {
            case RotateMode.loop:
                if(rotateTargetDirection > watchDirections.Length - 1)
                {
                    rotateTargetDirection = 0;
                    nextDirection = 0;
                }
                break;
            case RotateMode.backAndForth:
                if(rotatePreviousTarget >= rotateTargetDirection) //is moving back
                {
                    if(rotatePreviousTarget > 2)
                    {
                        nextDirection -= 2;
                    }
                }
                else if(rotateTargetDirection > watchDirections.Length - 1) //reached end, start moveback
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
        rotatePreviousTarget = rotateTargetDirection;
        rotateTargetDirection = pPosition;
        currentState = RotateState.rotatingToTarget;
    }

    void ReachedTarget()
    {
        currentState = RotateState.reachedTarget;
    }

    void AlertGuards()
    {
        for (int i = 0; i < guardsToAlert.Length; i++)
        {
            guardsToAlert[i].Alert(playersInVision[0].gameObject);
        }
    }

    void Alert()
    {
        isAlerted = true;
        AlertGuards();
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
