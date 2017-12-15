using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GuardCameraVision))]
public class GuardCameraVisionEditor : Editor {

    private void OnSceneGUI()
    {
        GuardCameraVision guardCameraVision = (GuardCameraVision)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(guardCameraVision.transform.position, Vector3.up, -Vector3.forward, guardCameraVision.viewAngle, guardCameraVision.viewRadius);


    }
}
