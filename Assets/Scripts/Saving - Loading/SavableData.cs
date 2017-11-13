using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavablePlayerData
{
    public Inventory inventory;
    public float playerPosX, 
                 playerPosY, 
                 playerPosZ;

}

[Serializable]
public class SavablePlayerProgressData
{
    public int savedSceneID;
}
