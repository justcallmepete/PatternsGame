using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour {

    public static Object prefab;

    public float radius;
    public float lifeTime;

    private float timer;

    public static SoundWave Create(float maxRadius, float lifeTime) {
        GameObject newObject = Instantiate(prefab) as GameObject;
        SoundWave soundWave = newObject.GetComponent<SoundWave>();
        soundWave.radius = maxRadius;
        soundWave.lifeTime = lifeTime;
        return soundWave;
    }

    // Use this for initialization
    void Awake()
    {
        prefab = Resources.Load("Prefabs/SoundRing");
    }

    private void Start()
    {
        gameObject.GetComponent<SphereCollider>().radius = this.radius;

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
