using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField]
    GameObject placement;

    Camera camera;
    [SerializeField]
    RectTransform canvasRect;
    [SerializeField]
    GameObject indicator;

    private void Start()
    {
        GameObject ob = GameObject.FindGameObjectWithTag("MainCamera");
        camera = ob.GetComponent<Camera>();
    }

    void Update()
    {
        ShowIndicator();
        //transform.LookAt(Camera.main.transform.position, -Vector3.up);
        //Vector3 camVector = transform.position;
        //camVector.x = camera.transform.position.x;
        //Quaternion newRotation = Quaternion.LookRotation(transform.position - camVector);

        //transform.rotation = newRotation;
    }

    void ShowIndicator()
    {
        // Final position of marker above GO in world space
        Vector3 offsetPos = new Vector3(placement.transform.position.x, placement.transform.position.y, placement.transform.position.z);
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = camera.WorldToScreenPoint(offsetPos);
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        indicator.transform.localPosition = canvasPos;
    }
}
