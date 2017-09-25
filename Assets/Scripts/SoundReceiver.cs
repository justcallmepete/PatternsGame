using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReciever : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SoundWave>() != null)
        {
            Debug.Log("Heard Something!");
        }
    }
}
