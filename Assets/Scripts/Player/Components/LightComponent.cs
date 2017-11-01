using System.Collections.Generic;
using UnityEngine;

public class LightComponent : PlayerComponentInterface
{
    public bool isInLight = false;

    public Material lightMaterial;
    public Material darkMaterial;

    public bool changedToLight = false;

    public override void AwakeComponent()
    {
        base.AwakeComponent();      
    }

    // Update is called once per frame
    public override void UpdateComponent()
    {
        base.UpdateComponent();
    }

    public override void LateUpdateComponent()
    {
        base.LateUpdateComponent();

        if (isInLight && !changedToLight)
        {
            Debug.Log("Player is in light");

            changedToLight = true;
            StartCoroutine(MainPlayer.AlphaFade(1));

            gameObject.GetComponent<Renderer>().material = lightMaterial;

        }
        else if (!isInLight && changedToLight)
        {
            Debug.Log("Player is in shadow");
            changedToLight = false;

            gameObject.GetComponent<Renderer>().material = darkMaterial;
        }
    }
}