using UnityEngine;

/*
 * Interface class for player components 
 */

public class PlayerComponentInterface : MonoBehaviour
{
    protected int id = 10; // Set default id value
    public int Id { get { return id; } }

    MainPlayer mainPlayer;

    public MainPlayer MainPlayer { get { return mainPlayer; } set { mainPlayer = value; } }

    public virtual void AwakeComponent() { }
    public virtual void UpdateComponent() { }
    public virtual void FixedUpdateComponent() { }
}
