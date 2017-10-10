using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePointBetweenPlayers : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;

    Transform posP1;
    Transform posP2;
    Vector3 MiddlePoint;

	// Use this for initialization
	void Start () {

        posP1 = player1.GetComponent<Transform>();
        posP2 = player2.GetComponent<Transform>();

       
	}
	
	// Update is called once per frame
	void Update () {

        MiddlePoint = (posP1.position + posP2.position) / 2;
        gameObject.GetComponent<Transform>().position = MiddlePoint;
	}

    public float getDistanceBetweenPlayers()
    {
        float dis;
        dis = Vector3.Distance(posP1.position, posP2.position);
        return dis;
    }



}
