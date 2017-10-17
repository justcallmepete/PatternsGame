using System;
using UnityEngine;

public class GuardVision : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpotPlayer(other.gameObject);
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

    private void SpotPlayer(GameObject player)
    {
        GetComponentInParent<GuardStateMachine>().Alert(player);
    }
}
