using System;
using UnityEngine;

public class GuardVision : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpotPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            LosePlayer(other.gameObject.transform.position);
        }
    }

    private void LosePlayer(Vector3 position)
    {
        GetComponentInParent<GuardStateMachine>().PlayerLost(position);
    }

    private void SpotPlayer()
    {
        GetComponentInParent<GuardStateMachine>().Alert();
    }
}
