using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class resizeLight : MonoBehaviour {

    [SerializeField]
    public Light light;
    public float cookieSize = 30;
    public BoxCollider collider;
    private float currentRatioNumerator;
    private float currentRatioDenominator;
    public float boxSizeMultiplier;

    void Start () {

    }
    public void ChangeRatio(int numerator, int denominator)
    {
        currentRatioNumerator = numerator;
        currentRatioDenominator = denominator;
        light = GetComponent<Light>();
        light.cookie = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Materials/Cookies/cookie" + numerator+"-" + denominator + ".png", typeof(Texture2D));
    }
    public void Rotate()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 0, 90));
    }
    public void SetColliderSize(float cookieSize)
    {
        if (currentRatioNumerator == 0) return;
        boxSizeMultiplier = 0.57f;
        collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(cookieSize * boxSizeMultiplier, ((cookieSize * boxSizeMultiplier) / currentRatioDenominator) * currentRatioNumerator, 2);
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
        light.SetColliderSize(light.cookieSize);
        if (GUI.changed)
        {
            EditorUtility.SetDirty(light);
        }
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
