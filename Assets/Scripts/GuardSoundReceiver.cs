using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This Class contains the guard logic for responding to sound. (Change state, etc.)
 */
public class GuardSoundReceiver : SoundReceiver {

    public override void OnSoundReceived()
    {
        Debug.Log("What was that?");
    }
}
