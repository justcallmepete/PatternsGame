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
    
    // Use this for initialization
    void Start () {
        guardCameraVision = transform.Find("GuardCameraVisualisation").gameObject;
        playersInVision = new List<MainPlayer>();
        visionMaterial = guardCameraVision.GetComponentInChildren<Renderer>().material;

        Debug.Log("name: " + visionMaterial.name);
        isAlerted = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isAlerted)
        {
            //TO DO: Check if no players in sight   
            if(playersInVision.Count <= 0)
            {
                SetSemiAlert();
            }
            return;
        }

        if (playerInLight)
        {
            Alert();
        }
	}
    void Alert()
    {
        isAlerted = true;
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
