using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whistle : MonoBehaviour {


    public float radius;
    public float lifeTime;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            SoundWave obj = SoundWave.Create(radius, lifeTime);
            obj.transform.position = this.transform.position;
        }
	}
}
