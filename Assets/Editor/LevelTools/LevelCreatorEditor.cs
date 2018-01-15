using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCreator))]
[CanEditMultipleObjects]
public class LevelCreatorEditor : Editor {
    LevelCreator levelCreator;

    SerializedProperty testValue;
    enum MenuStates { buildBase = 0, mainMenu = 1, buildRoom = 2};
    MenuStates currentState;
    void OnEnable()
    {
        testValue = serializedObject.FindProperty("testValue"); 
    }

    public override void OnInspectorGUI()
    {
        levelCreator = (LevelCreator)target;
        if (!levelCreator.levelBaseMain)
        {
            DrawBuildBaseScreen();
            return;
        }
        switch (currentState) {
            case MenuStates.mainMenu:
                DrawMainScreen();
                break;
            case MenuStates.buildRoom:
                if (levelCreator.roomBeingBuild == null)
                {
                    currentState = MenuStates.mainMenu;
                    break;
                }
                DrawBuildRoomScreen();
                break;
            default:
                DrawBuildRoomScreen();
                break;
        }
    }

    void DrawBuildBaseScreen()
    {
        currentState = MenuStates.buildBase;
        if (GUILayout.Button("Build Base"))
        {
            levelCreator.SpawnBaseLevel();
            currentState = MenuStates.mainMenu;
        }
    }

    void DrawMainScreen()
    {
        Rect R = EditorGUILayout.BeginHorizontal("Button");
        if (GUILayout.Button("Build Room",GUILayout.Height(100)))
        {
            BuildNewRoom();
        }
        if (GUILayout.Button("Build Door",GUILayout.Height(100)))
        {

        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawBuildRoomScreen()
    {
        EditorStyles.textField.wordWrap = true;
        EditorGUILayout.TextArea("Select ratio/scale and light and confirm to make a room");

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //Ratio region
        #region

        float buttonWidth = 50f;
        float buttonHeight = 50f;
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select Room ratio");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select 1:1", GUILayout.MinWidth(buttonWidth), GUILayout.MinHeight(buttonHeight)))
        {
            levelCreator.SetRoomRatio(1, 1);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select 7:8", GUILayout.MinWidth(buttonWidth), GUILayout.MinHeight(buttonHeight)))
        {
            levelCreator.SetRoomRatio(7, 8);
        }
        GUILayout.FlexibleSpace();  
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select 3:4", GUILayout.MinWidth(buttonWidth), GUILayout.MinHeight(buttonHeight)))
        {
            levelCreator.SetRoomRatio(3, 4);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select 5:8", GUILayout.MinWidth(buttonWidth), GUILayout.MinHeight(buttonHeight)))
        {
            levelCreator.SetRoomRatio(5, 8);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select 1:2", GUILayout.MinWidth(buttonWidth), GUILayout.MinHeight(buttonHeight)))
        {
            levelCreator.SetRoomRatio(1, 2);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select 3:8", GUILayout.MinWidth(buttonWidth), GUILayout.MinHeight(buttonHeight)))
        {
            levelCreator.SetRoomRatio(3, 8);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //Scale region
        #region

        float roomScale = EditorGUILayout.Slider("Room size", levelCreator.setRoomScale, 10,120);
        levelCreator.setRoomScale = roomScale;
        #endregion
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Rotate room"))
        {
            levelCreator.ToggleRotateRoom();
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //Confirm and deny region
        #region
        GUIContent confirmPlacement = new GUIContent("\u2611", "Confirm room placement");
        GUIContent denyPlacement = new GUIContent("\u2612", "deny room placement");


        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Confirm or deny placement");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUI.color = Color.green;
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(confirmPlacement, EditorStyles.miniButtonLeft, GUILayout.MinWidth(buttonWidth/2), GUILayout.MinHeight(buttonHeight/2)))
        {
            levelCreator.ConfirmRoomPlacement();
            currentState = MenuStates.mainMenu;
        }
        GUI.color = Color.red;
        if (GUILayout.Button(denyPlacement, EditorStyles.miniButtonRight, GUILayout.MinWidth(buttonWidth/2), GUILayout.MinHeight(buttonHeight/2)))
        {
            levelCreator.DenyRoomPlacement();
            currentState = MenuStates.mainMenu;
            ActiveEditorTracker.sharedTracker.isLocked = false;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
#endregion
    }

    void BuildNewRoom()
    {
        levelCreator.SpawnRoom();
        currentState = MenuStates.buildRoom;
    }

}
