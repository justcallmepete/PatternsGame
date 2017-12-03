using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheckComponent : PlayerComponentInterface {

    public bool lightIsActive;

    private void OnTriggerEnter(Collider other)
    {
        if(lightIsActive && other.gameObject.GetComponent<MainPlayer>() != null)
        {
            other.gameObject.GetComponent<MainPlayer>().lightStandingIn.Add(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lightIsActive && other.gameObject.GetComponent<MainPlayer>() != null)
        {
            other.gameObject.GetComponent<MainPlayer>().lightStandingIn.Remove(this.gameObject);
        }
    }

}
