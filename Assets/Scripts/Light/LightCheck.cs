using System.Collections.Generic;
using UnityEngine;

/*
 * Checks to see if one of the players is in the light. Adds lights to an array stored in the player to determine if the player is in light.
 */
public class LightCheck : MonoBehaviour {

    public bool LightIsActive { get { return lightIsActive; } set { lightIsActive = value; } }
    public bool lightIsActive = true;

    private Light myLight;

    List<GuardStateMachine> guardsInMe;
    public bool isOffOnStart = false;

    private void Awake()
    {
        myLight = GetComponent<Light>();
    }

    private void Start()
    {
        if (isOffOnStart)
        {
            lightIsActive = false;
            myLight.enabled = false;
        }
        guardsInMe = new List<GuardStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(lightIsActive && other.gameObject.GetComponent<MainPlayer>() != null)
        {
            other.gameObject.GetComponent<MainPlayer>().lightStandingIn.Add(this.gameObject);
        }

        GuardStateMachine gsm = other.gameObject.GetComponentInParent<GuardStateMachine>();
        if (lightIsActive && gsm != null)
        {
            gsm.lightsStandingIn.Add(this.gameObject);
            guardsInMe.Add(gsm);
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
            //Debug.Log("no light");
            gsm.lightsStandingIn.Remove(this.gameObject);
            guardsInMe.Remove(gsm);
        }
    }

    public void ToggleLight()
    {
        if (!lightIsActive)
        {
            lightIsActive = true;
            myLight.enabled = true;
        }
        else
        {
            lightIsActive = false;
            myLight.enabled = false;
            for (int i = 0; i < guardsInMe.Count; i++)
            {
                guardsInMe[i].lightsStandingIn.Remove(this.gameObject);
            }
            guardsInMe = new List<GuardStateMachine>();
        }
    }

}
