using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This file is used to define classes of savable data. 
 * 
 * Because it is in the basis a new datatype, you have to define it to be serializable, otherwise it cannot be used in the save file. 
 */ 

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
