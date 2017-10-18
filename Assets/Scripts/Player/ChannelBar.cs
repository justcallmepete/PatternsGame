using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelBar : MonoBehaviour {

    Teleportation teleportation;
    Transform channelBar;

	// Use this for initialization
	void Start () {
        teleportation = gameObject.GetComponentInParent<Teleportation>();
        channelBar = gameObject.transform.Find("ChannelBar");
	}
	
	// Update is called once per frame
	void Update () {

        float channelRatio = teleportation.GetChannelTimeRatio();
        if (channelRatio == 0)
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        channelBar.transform.localScale = new Vector3(teleportation.GetChannelTimeRatio(), 1);
    }
}
