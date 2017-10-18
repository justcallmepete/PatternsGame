using UnityEngine;

/*
 * Rotate the canvas towards the camera
 */ 

public class CanvasLookAtCamera : MonoBehaviour {

    public Camera camera;

	void Start () {
        camera = Camera.main;
	}
	

	void Update () {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.down);
	}
}
