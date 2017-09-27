using UnityEngine;

/**
 * This abstract
 */
public abstract class SoundReceiver : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SoundWave>() != null)
        {
            OnSoundReceived();
        }
    }

    public abstract void OnSoundReceived();

    
}
