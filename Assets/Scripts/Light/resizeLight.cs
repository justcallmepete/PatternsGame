using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class resizeLight : MonoBehaviour {

    public Texture2D cookie;
    public Bounds bounds;
    Light light;
    float cookieSize;
    float biggerSize;
	// Use this for initialization
	void Start () {
        bounds = this.GetComponent<MeshFilter>().mesh.bounds;
        light = GetComponentInChildren<Light>();
        cookie = (Texture2D)light.cookie;
        cookieSize = light.cookieSize;
        if(this.transform.localScale.x>= this.transform.localScale.z)
        {
            print(this.transform.localScale.x);
            biggerSize = this.transform.localScale.x;
        }
        else
        {
            print(this.transform.localScale.z);
            biggerSize = this.transform.localScale.z;
        }
        light.cookieSize = light.cookieSize * biggerSize;

        light.enabled = true;
        this.GetComponent<Renderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
