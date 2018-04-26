using UnityEngine;
/** Abstract class for Guard States. All states for guards inherit from this class*/
[System.Serializable]
public abstract class GuardState {

    protected GuardStateMachine context;

    public GuardState(GuardStateMachine context)
    {
        this.context = context;
    }

    public abstract void OnStateEnter();
    public abstract void Update();
    public abstract void OnStateExit();

    /* What happens on discraction */
    public abstract void OnDistraction(Vector3 target);
    /* What happens when a player is seen */
    public abstract void OnSeePlayer(GameObject player);
}
