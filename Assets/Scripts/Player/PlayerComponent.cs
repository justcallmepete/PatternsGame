using UnityEngine;

/*
 * 
 */
 
public class PlayerComponent : MonoBehaviour {

    MainPlayer mainPlayer;

    public MainPlayer MainPlayer { get { return mainPlayer; } set { mainPlayer = value; } }

    public virtual void AwakeComponent() { }
    public virtual void UpdateComponent() { }
    public virtual void FixedUpdateComponent() { }
}
