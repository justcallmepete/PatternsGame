using UnityEngine;

/*
 * This class is used to add behaviour to the camera.  
 * 
 */

public class CameraBehaviour : MonoBehaviour
{
    
    [SerializeField]
    GameObject target; // The focus point (middle point between the players) 

    Transform targetPos;
    Transform camPos;
    Camera camera;
    float distance;

    [Header("The delay in the response of the zoom of the camera (lerp) (higher is less delay)")]
    [SerializeField]
    [Range(0, 5)]
    float speed = 2.5f;

    [SerializeField]
    [Range(1, 2)]
    float zoomSpeed = 1.125f;

    private void Awake()
    {
        // Defining the start position of the camera
        gameObject.GetComponent<Transform>().position = new Vector3(-20, 50, -20); 
    }

    private void Start()
    {
        targetPos = target.GetComponent<Transform>();
        camPos = gameObject.GetComponent<Transform>();
        camera = gameObject.GetComponent<Camera>();
        target = GameObject.Find("Camera");
    }

    void Update()
    {
        

        camPos.LookAt(targetPos);

        distance = target.GetComponent<MiddlePointBetweenPlayers>().getDistanceBetweenPlayers();

        // Zooming in and out, adding lerp effect to it. 
        float step = speed * Time.deltaTime;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, camera.fieldOfView = distance * zoomSpeed, step);


        if (camera.fieldOfView < 30)
        {
            camera.fieldOfView = 30;
        }
        else if (camera.fieldOfView > 60)
        {
            camera.fieldOfView = 60;
        }


    }
    
}
