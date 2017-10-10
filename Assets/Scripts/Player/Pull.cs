using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pull : MonoBehaviour
{
    [Header("Key settings")]
    public byte pullToPlayerKey = 4;
    public byte pullPlayerKey = 5;
    public float maxDistance;

    private Controlable controlable;
    private List<GameObject> otherPlayers = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        controlable = gameObject.GetComponent<Controlable>();
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject obj in allPlayers)
        {
            if (obj != gameObject)
            {
                otherPlayers.Add(obj);
            }
        }
    }

    private void Update()
    {
        if (otherPlayers.Count == 0)
        {
            return;
        }

        if (controlable.GetButtonDown(pullToPlayerKey))
        {

            if (IsPlayerInSight())
            {
                PullToPlayer();
            }
        }
        if (controlable.GetButtonDown(pullPlayerKey))
        {
            if (IsPlayerInSight())
            {
                PullPlayer();
            }
        }

    }

    private bool IsPlayerInSight()
    {
        RaycastHit hit;

        Vector3 target = (GetClosestPlayer().transform.position - transform.position).normalized * maxDistance;
        Debug.DrawRay(transform.position, target, Color.green);

        if (Physics.Raycast(transform.position, target, out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetClosestPlayer()
    {
        int index = 0;
        float minDistance = Vector3.Distance(otherPlayers[0].transform.position, gameObject.transform.position);

        for (int i = 1; i < otherPlayers.Count; i++)
        {
            float distance = Vector3.Distance(otherPlayers[i].transform.position, gameObject.transform.position);

            if (distance < minDistance)
            {
                index = i;
                minDistance = distance;
            }
        }

        return otherPlayers[index];
    }

    private void PullPlayer()
    {
        GetClosestPlayer().GetComponent<MainPlayer>().BePulled(gameObject);
    } 

    private void PullToPlayer()
    {
       gameObject.GetComponent<MainPlayer>().BePulled(GetClosestPlayer());
    }
    
}
