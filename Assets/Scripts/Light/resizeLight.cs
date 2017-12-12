using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class resizeLight : MonoBehaviour {

    // [Range(0, 2)]
    // public float cookieSizeIncreaser = 1;
    // public Texture2D cookie;
    // public Bounds bounds;
    public Light light;
    public float cookieSize = 30;
    // float biggerSize;
	// Use this for initialization
	void Start () {
        // bounds = this.GetComponent<MeshFilter>().mesh.bounds;
        // cookie = (Texture2D)light.cookie;
        // cookieSize = light.cookieSize;
        // if(this.transform.localScale.x>= this.transform.localScale.z)
        // {
        //     print(this.transform.localScale.x);
        //     biggerSize = this.transform.localScale.x;
        // }
        // else
        // {
        //     print(this.transform.localScale.z);
        //     biggerSize = this.transform.localScale.z;
        // }
        // light.cookieSize = light.cookieSize * (biggerSize * cookieSizeIncreaser);
        // 
        // light.enabled = true;
        // this.GetComponent<Renderer>().enabled = false;
    }
    public void ChangeRatio(int numerator, int denominator)
    {
        light = GetComponent<Light>();
        light.cookie = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Materials/Cookies/cookie" + numerator+"-" + denominator + ".png", typeof(Texture2D));
    }
    public void Rotate()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 0, 90));
    }

    // Update is called once per frame
    void Update () {
		
	}
}

[CustomEditor(typeof(resizeLight))]
public class LightEditor: Editor
{
    public override void OnInspectorGUI()
    {
        resizeLight light = (resizeLight)target;
        light.cookieSize = EditorGUILayout.Slider("Cookie size", light.cookieSize, 10, 100);
        light.gameObject.GetComponent<Light>().cookieSize = light.cookieSize;
        if (GUILayout.Button("Rotate cookie"))
        {
            light.Rotate();
        }
        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select 1:1"))
            {
                light.ChangeRatio(1, 1);
            }
            if (GUILayout.Button("Select 7:8"))
            {
                light.ChangeRatio(7, 8);
            }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select 3:4"))
            {
                light.ChangeRatio(3, 4);
            }
            if (GUILayout.Button("Select 5:8"))
            {
                light.ChangeRatio(5, 8);
            }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select 1:2"))
            {
                light.ChangeRatio(1, 2);
            }
            if (GUILayout.Button("Select 3:8"))
            {
                light.ChangeRatio(3, 8);
            }
        GUILayout.EndHorizontal();
    }
}
