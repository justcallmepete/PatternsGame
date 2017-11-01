using System.Collections.Generic;
using UnityEngine;

public class LightComponent : PlayerComponentInterface
{
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

        if (MainPlayer.IsInLight)
        {
            Debug.Log("Player is in light");
        }
        else
        {
            Debug.Log("Player is in shadow");

        }
    }
}