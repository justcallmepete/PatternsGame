using UnityEngine;

/**
 * This abstract
 */
public abstract class SoundReceiver : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SoundWave>() != null)
        {
            OnSoundReceived(other.gameObject);
        }
    }

    public abstract void OnSoundReceived(GameObject source);

    
}
