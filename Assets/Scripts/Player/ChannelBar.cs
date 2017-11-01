using UnityEngine;

/*
 * Get the channel time ratio and show the channel bar to the player
 */ 

public class ChannelBar : MonoBehaviour
{
    MainPlayer mainPlayer;
    Transform channelBar;

    void Start()
    {
        // Cache components for later use
        mainPlayer = gameObject.GetComponentInParent<MainPlayer>();
        channelBar = gameObject.transform.Find("ChannelBar");
    }

    void Update()
    {
        float channelRatio = mainPlayer.ChannelTimeRatio;

        // Change visibility when the ratio is zero
        if (channelRatio == 0)
        {
            gameObject.GetComponent<Canvas>().enabled = false;  
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        // Apply scaling of the bar
        channelBar.transform.localScale = new Vector3(channelRatio, 1);
    }
}
