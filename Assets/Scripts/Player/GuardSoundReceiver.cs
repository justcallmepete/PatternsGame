using UnityEngine;

/*
 * This Class contains the guard logic for responding to sound. (Change state, etc.)
 */
public class GuardSoundReceiver : SoundReceiver {

    public override void OnSoundReceived(GameObject source)
    {
        GuardStateMachine stateMachine = GetComponent<GuardStateMachine>();
        if (stateMachine)
        {
            stateMachine.Distract(source.transform.position);
        }
    }
}
