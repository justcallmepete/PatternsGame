using UnityEngine;

/*
 * This script will change the material of the player if the player stands in the light or shadow. 
 * It checks the variable IsStandingInLight in the MainPlayer class.
 */ 

public class LightComponent : PlayerComponentInterface
{
    [Tooltip("Material when standing in a light")]
    public Material lightMaterial;
    [Tooltip("Material when standing in the shadow")]
    public Material darkMaterial;

    // Light state: In light or in shadow
    public enum LightState
    {
        Light,
        Shadow
    }

    public LightState lightState = LightState.Light;

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        // Set id
        id = 11;

        // Set material when starting a new level.
        if (MainPlayer.IsStandingInLight)
        {
            SetLightState(LightState.Light);
            SetNewMaterial(lightMaterial);
        }
        else
        {
            SetLightState(LightState.Shadow);
            SetNewMaterial(darkMaterial);
        }
    }

    public override void LateUpdateComponent()
    {
        base.LateUpdateComponent();

        if (MainPlayer.IsStandingInLight && lightState != LightState.Light )
        {
            SetLightState(LightState.Light);
            SetNewMaterial(lightMaterial);
        }
        else if (!MainPlayer.IsStandingInLight && lightState != LightState.Shadow)
        {
            SetLightState(LightState.Shadow);
            SetNewMaterial(darkMaterial);
        }
    }

    private void SetLightState(LightState pState)
    {
        lightState = pState;
    }

    private void SetNewMaterial(Material pMaterial)
    {
        gameObject.GetComponent<Renderer>().material = pMaterial;
    }
}