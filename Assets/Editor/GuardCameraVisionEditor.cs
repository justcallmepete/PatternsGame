using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GuardCameraVision))]
public class GuardCameraVisionEditor : Editor {

    private void OnSceneGUI()
    {
        GuardCameraVision guardCameraVision = (GuardCameraVision)target;
        Handles.color = new Color(255, 0, 0, 0.25f);
        float angle = guardCameraVision.transform.eulerAngles.y - guardCameraVision.viewAngle / 2;
        Vector3 startDir = Mainframe.utils.MathUtils.DirFromAngle(angle, true, guardCameraVision.transform);
        Vector3 startPos = guardCameraVision.transform.position;
        startPos.y = 0.1f;
        Handles.DrawSolidArc(startPos, Vector3.up, startDir, guardCameraVision.viewAngle, guardCameraVision.viewRadius);
    }
}
