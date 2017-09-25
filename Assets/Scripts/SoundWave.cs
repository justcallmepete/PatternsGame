using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for the soundwaves. 
 */
public class SoundWave : MonoBehaviour {

    public static Object prefab;

    public float radius;
    public float lifeTime;

    private float timer;

    // Factory Method for SoundWave objects. Returns a instace of the SoundRing prefab with specified parameters.
    public static SoundWave Create(float maxRadius, float lifeTime) {
        GameObject newObject = Instantiate(prefab) as GameObject;
        SoundWave soundWave = newObject.GetComponent<SoundWave>();
        soundWave.radius = maxRadius;
        soundWave.lifeTime = lifeTime;
        return soundWave;
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<SphereCollider>().radius = this.radius;
        prefab = Resources.Load("Prefabs/SoundRing");
    }

    // Update is called once per frame
    void Update () {
        
        timer += Time.deltaTime;
        if (this.timer >= this.lifeTime)
        {
            GameObject.Destroy(gameObject);
        }
	}
}
