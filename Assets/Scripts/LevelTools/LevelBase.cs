using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour {

    [HideInInspector]
    public float editorSize;
    [HideInInspector]
    public int numerator, denominator;

    private void OnEnable()
    {
        editorSize = 0;
        numerator = 1;
        denominator = 1;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateSize(bool isRotated = false, float pSize = Mathf.Infinity)
    {
        //update numerator and denominator
        if (isRotated)
        {
            transform.localScale = new Vector3(editorSize, 4, editorSize / denominator * numerator);
        }
        else
        {
            transform.localScale = new Vector3(editorSize / denominator * numerator, 4, editorSize);
        }
    }

    public void ConfirmPlacement()
    {

    }
}
