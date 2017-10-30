
using UnityEngine;

public class WhistleComponent : PlayerComponent {

    //Button Code for activating the ability.
    public int button;
    //Radius of the soundwave
    public float radius = 4;
    //How long does the sound stay active?
    public float lifeTime = 0.5f;
    //Player 1/2/3/etc
    public override void UpdateComponent()
    {
        base.UpdateComponent();
        if (MainPlayer.buttonDownList[button])
        {
            SoundWave obj = SoundWave.Create(radius, lifeTime);
            obj.transform.position = this.transform.position;
        }
    }
}
