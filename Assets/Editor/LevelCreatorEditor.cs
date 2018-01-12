using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCreator))]
[CanEditMultipleObjects]
public class LevelCreatorEditor : Editor {

    SerializedProperty testValue;

    void OnEnable()
    {
        testValue = serializedObject.FindProperty("testValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(testValue);
        serializedObject.ApplyModifiedProperties();
        LevelCreator levelCreator = (LevelCreator)target;
        if (!levelCreator.levelBaseMain)
        {
            if (GUILayout.Button("Build Base"))
            {
                levelCreator.SpawnBaseLevel();
            }
            return;
        }
        else
        {
            DrawDefaultInspector();
            //DrawBuildRoomScreen();
        }
    }

    void DrawBuildRoomScreen()
    {
        Rect R = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(R, GUIContent.none)) {
            Debug.Log("create room");
        };
        GUILayout.Label("Im inside the button");
        EditorGUILayout.EndHorizontal();
    }

}
