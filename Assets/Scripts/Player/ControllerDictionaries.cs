using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDictionaries : MonoBehaviour {
       /*
    * 0 - X
    * 1 - A
    * 2 - B
    * 3 - Y
    * 4 - L1
    * 5 - R1
    * 6 - L2
    * 7 - R2
    * 8 - Select
    * 9 - Start
    */
    public Dictionary<string, int> ps4 = new Dictionary<string, int>()
    {
        {"X", 0},
        {"A", 1},
        {"B", 2},
        {"Y", 3},
        {"L1", 4},
        {"R1", 5},
        {"L2", 6},
        {"R2", 7},
        {"Select", 8},
        {"Start", 9}
    };
    public Dictionary<string, int> xbox = new Dictionary<string, int>()
    {
        {"X", 2},
        {"A", 0},
        {"B", 1},
        {"Y", 3},
        {"L1", 4},
        {"R1", 5},
        {"L2", 8},
        {"R2", 9},
        {"Select", 6},
        {"Start", 7}
    };
    public Dictionary<string, int> steam = new Dictionary<string, int>()
    {
        {"X", 0},
        {"A", 1},
        {"B", 2},
        {"Y", 3},
        {"L1", 4},
        {"R1", 5},
        {"L2", 6},
        {"R2", 7},
        {"Select", 8},
        {"Start", 9}
    };
    public Dictionary<string, int> keyboard = new Dictionary<string, int>()
    {
        {"X", 0},
        {"A", 1},
        {"B", 2},
        {"Y", 3},
        {"L1", 4},
        {"R1", 5},
        {"L2", 6},
        {"R2", 7},
        {"Select", 8},
        {"Start", 9}
    };
}
