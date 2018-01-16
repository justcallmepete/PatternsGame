using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorInfo : MonoBehaviour {

    private static LevelCreatorInfo _instance;

    public static LevelCreatorInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("LevelCreatorInfo");
                go.AddComponent<LevelCreatorInfo>();
            }

            return _instance;
        }
    }

    public float baseLenght = 100;
    public float baseWidth = 100;

    public float wallHeight = 300;

    public LevelBase levelBase;
    public Room basicRoom;
    public Door doorPrefab;

    private void Awake()
    {
        _instance = this;
    }
}
