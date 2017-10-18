using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    public bool isInLight;

	// Use this for initialization
	void Start () {
        isInLight = false;

	}
	
	// Update is called once per frame
	void Update () {

	}

    void LateUpdate()
    {
        isInLight = false;
    }
}
