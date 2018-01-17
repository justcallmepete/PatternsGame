﻿using UnityEngine;

/*
 * Checks to see if one of the players is in the light. Adds lights to an array stored in the player to determine if the player is in light.
 */
public class LightCheck : MonoBehaviour {

    public bool LightIsActive { get { return lightIsActive; } set { lightIsActive = value; } }
    public bool lightIsActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if(lightIsActive && other.gameObject.GetComponent<MainPlayer>() != null)
        {
            other.gameObject.GetComponent<MainPlayer>().lightStandingIn.Add(this.gameObject);
        }

        GuardStateMachine gsm = other.gameObject.GetComponentInParent<GuardStateMachine>();
        if (lightIsActive && gsm != null)
        {
            gsm.InLight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lightIsActive && other.gameObject.GetComponent<MainPlayer>() != null)
        {
            other.gameObject.GetComponent<MainPlayer>().lightStandingIn.Remove(this.gameObject);
        }

        GuardStateMachine gsm = other.gameObject.GetComponentInParent<GuardStateMachine>();
        if (lightIsActive && gsm != null)
        {
            Debug.Log("no light");
            gsm.InLight = false;
        }
    }

}
