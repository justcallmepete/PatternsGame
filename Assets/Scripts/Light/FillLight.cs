using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FillLight : MonoBehaviour {

    public new Light light;

    public int cookieWidth = 500;
    public int cookieHeight = 500;
    public float gradientLength = 100;
    public Texture2D cookie;
    public TextureImporter tImporter;

    // Use this for initialization
    void Start () {

        cookie = new Texture2D(cookieWidth, cookieHeight, TextureFormat.ARGB32, false);
        cookie.wrapMode = TextureWrapMode.Clamp;

        DrawPixels(cookie.width, cookie.height);

        // this.GetComponent<Renderer>().material.mainTexture = cookie;
     
        byte[] bytes = cookie.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Materials/Cookies/cookie6.png", bytes);

        SetTextureImporterFormat(Application.dataPath + "/Materials/Cookies/cookie6.png", true);
        
        light = this.transform.GetChild(0).gameObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.transform.Rotate(new Vector3(90, 0, 0));
        light.cookie = cookie;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DrawPixels (int width, int height)
    {
        FillWithColor(width, height, Color.white);
        float colorIncreaser = 0;

        Color color = Color.black;

        for (int x = 0; x <= gradientLength; x++)
        {
            for (int y = 0 + x; y <= height - x; y++)
            {               
                cookie.SetPixel(x, y, color);
                cookie.SetPixel(y, x, color);
            }
            if (colorIncreaser < 255)
            {
                colorIncreaser += (255 / gradientLength);
                color = new Color(colorIncreaser / 255, colorIncreaser / 255, colorIncreaser / 255);
            }
        }
        colorIncreaser = 0;
        for (int x = width; x >= width-gradientLength; x--)
        {
            for (int y = (height - (width - x)); y >= (0 +  (width - x)); y--)
            {
                cookie.SetPixel(x, y, color);
                cookie.SetPixel(y, x, color);
            }
            if (colorIncreaser < 255)
            {
                colorIncreaser += (255 / gradientLength);
                color = new Color(colorIncreaser / 255, colorIncreaser / 255, colorIncreaser / 255);
            }
        }
        cookie.Apply();
    }

    private void FillWithColor(float width, float height, Color color)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                cookie.SetPixel(x, y, color);

            }
        }
    }

    public void SetTextureImporterFormat(string path, bool isReadable)
    {
        // if (null == texture) return;

        //string assetPath = AssetDatabase.GetAssetPath(texture);

        tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        Debug.Log(path);
        if (tImporter != null)
        {
            Debug.Log("hoi");
            tImporter.textureType = TextureImporterType.Advanced;
            tImporter.isReadable = isReadable;
            tImporter.textureType = TextureImporterType.Cookie;
            tImporter.alphaSource = TextureImporterAlphaSource.FromGrayScale;

            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }
}
