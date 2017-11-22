using UnityEngine;

/*
 * Create a soundwave when the whistle key is pressed. The soundwave should be in the Resource folder.
 */

public class WhistleComponent : PlayerComponentInterface {

    [Tooltip("Button Code for activating the ability.")]
    public string whistleButton = "Y";
    private int whistleKey; 
    [Tooltip("Radius of the soundwave.")]
    public float radius = 4;
    [Tooltip("Life time of the soundwave")]
    public float lifeTime = 0.5f;

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        // Set id
        id = 3;

        whistleKey = InputManager.Instance.GetKey(whistleButton, MainPlayer.GetPlayerIndex());
    }


    public override void UpdateComponent()
    {
        base.UpdateComponent();

        if (MainPlayer.buttonDownList[whistleKey])
        {
            SoundWave obj = SoundWave.Create(radius, lifeTime);
            obj.transform.position = this.transform.position;
        }
            
    }
}
