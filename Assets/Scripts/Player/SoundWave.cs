using UnityEngine;
using EazyTools.SoundManager;
/*
 * Class for the soundwaves. 
 */
public class SoundWave : MonoBehaviour {

    public static Object prefab;

    public float radius;
    public float lifeTime;

    [Tooltip("Audio for whistle")]
    public AudioClip whistleSound;

    private float timer;

    // Factory Method for SoundWave objects. Returns a instace of the SoundRing prefab with specified parameters.
    public static SoundWave Create(float maxRadius, float lifeTime) {
        if (prefab ==  null) prefab = Resources.Load("Prefabs/SoundRing");
        GameObject newObject = Instantiate(prefab) as GameObject;
        SoundWave soundWave = newObject.GetComponent<SoundWave>();
        soundWave.radius = maxRadius;
        soundWave.lifeTime = lifeTime;
        return soundWave;
    }

    // Use this for initialization
    void Start()
    {
        SoundManager.PlaySound(whistleSound, false);
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
