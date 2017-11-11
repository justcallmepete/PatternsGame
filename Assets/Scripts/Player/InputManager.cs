﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

    public static InputManager _instance;
    private GameObject[] players;
    private ControllerDictionaries controllerDictionaries;
    private string[] controllerNames;
    public string[] actualControllerNames;

    public enum ControllerType
    {
        PS4,
        XBoxOne,
        Steam,
        Keyboard
    }
    public ControllerType[] controllerUsed;
    private bool [] checkedController;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameObject = new GameObject("InputManager");
                gameObject.AddComponent<InputManager>();
                _instance = gameObject.GetComponent<InputManager>();
            }
    
            return _instance;
        }
    }

    // Use this for initialization
    void Awake () {
        controllerDictionaries = new ControllerDictionaries();
        actualControllerNames = new string[2];

        controllerUsed = new ControllerType[2];
        controllerUsed[0] = ControllerType.Keyboard;
        controllerUsed[1] = ControllerType.Keyboard;

        checkedController = new bool[2];
        checkedController[0] = false;
        checkedController[1] = false;
    }
	
	// Update is called once per frame
	void Update () {
        SetControllersUsed(0);
        SetControllersUsed(1);
    }

    private void SetControllersUsed(int index)
    {
        checkedController[index] = true;
        controllerNames = Input.GetJoystickNames();

        actualControllerNames[0] = controllerNames[0];
        actualControllerNames[1] = controllerNames[1];
        if (actualControllerNames[index] == "")
        {
            //print("NO CONTROLLER FOR PLAYER "+index);
            controllerUsed[index] = ControllerType.Keyboard;
            return;
        }
        switch (actualControllerNames[index].Length)
        {
            case 19:
                //print("PS4 CONTROLLER IS CONNECTED FOR PLAYER " + index);
                controllerUsed[index] = ControllerType.PS4;
                break;
            case 33:
                //print("XBOX ONE CONTROLLER IS CONNECTED FOR PLAYER " + index);
                controllerUsed[index] = ControllerType.XBoxOne;
                break;
        }     
    }

    public int GetKey(string button, int index)
    {
        if (!checkedController[index]) {
            SetControllersUsed(index);
        }
        int key = 0;
        switch(controllerUsed[index])
        {
            case ControllerType.PS4:
                key = controllerDictionaries.ps4[button];
                break;
            case ControllerType.XBoxOne:
                key = controllerDictionaries.xbox[button];
                break;
            case ControllerType.Steam:
                key = controllerDictionaries.steam[button];
                break;
            case ControllerType.Keyboard:
                key = controllerDictionaries.keyboard[button];
                break;
        }
        return (key);
    }
    public ControllerType CheckControllerType(int index)
    {
        ControllerType controllerType = controllerUsed[index];
        return controllerType;
    }
}
