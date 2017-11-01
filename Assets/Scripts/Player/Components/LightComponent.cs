using UnityEngine;

public class LightComponent : PlayerComponentInterface
{
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

        if (MainPlayer.IsStandingInLight && !changedToLight)
        {
            changedToLight = true;
            StartCoroutine(MainPlayer.AlphaFade(1));

            gameObject.GetComponent<Renderer>().material = lightMaterial;

        }
        else if (!MainPlayer.IsStandingInLight && changedToLight)
        {
            changedToLight = false;
            gameObject.GetComponent<Renderer>().material = darkMaterial;
        }
    }
}