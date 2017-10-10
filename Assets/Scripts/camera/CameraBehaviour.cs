using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField]
    GameObject target;

    Transform targetPos;
    Transform camPos;
    Camera camera;
    float distance;

    private void Awake()
    {
        gameObject.GetComponent<Transform>().position = new Vector3(-20, 50, -20);
    }
    private void Start()
    {
        targetPos = target.GetComponent<Transform>();
        camPos = gameObject.GetComponent<Transform>();
        camera = gameObject.GetComponent<Camera>();

    }
    // Update is called once per frame
    void Update()
    {

        camPos.LookAt(targetPos);

        distance = target.GetComponent<MiddlePointBetweenPlayers>().getDistanceBetweenPlayers();

        camera.fieldOfView = distance * 1.125f;

        if (camera.fieldOfView < 35)
        {
            camera.fieldOfView = 35;
        }
        else if (camera.fieldOfView > 60)
        {
            camera.fieldOfView = 60;
        }
        else
        {
            
        }


    }
    
}
