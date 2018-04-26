using UnityEngine;

/*
    Class is used to define the middle point between the players. A third object can be added to focus to as well. 
    (middle point between players and third object)
*/
public class MiddlePointBetweenPlayers : MonoBehaviour {

    [Header ("Focus points. Players are added automatically")]
    [SerializeField]
    Transform player1;
    [SerializeField]
    Transform player2;
    [Header ("Add this at runtime using setFocusObject(GameObject obj);")]
    [SerializeField]
    Transform focusObject;
    GameObject[] players;

    Vector3 middlePoint;

    [Header ("The delay in the response of the position ")]
    [Header ("of the camera (lerp) (higher is less delay)")]
    [SerializeField]
    [Range(0, 5)]
    float speed = 2.5f;


	void Start () {

        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 2)
        {
            Debug.Log("too many players found!");
        }
        else
        {
            player1 = players[0].transform;
            player2 = players[1].transform;
        }
    }
	
	void Update () {

        if(focusObject == null)
        {
            middlePointBetweenPlayers(player1.position, player2.position);
        }
        else if(focusObject != null)
        {
            middlePointBetweenPlayersAndObject(player1.position, player2.position, focusObject.position);
        }
        // Adding a lerp effect
        float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, middlePoint, step);
    }

    Vector3 middlePointBetweenPlayers(Vector3 pos1, Vector3 pos2)
    {
        middlePoint = (pos1 + pos2) / 2;
        return middlePoint;
    }

    Vector3 middlePointBetweenPlayersAndObject(Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        middlePoint = (pos1 + pos2 + pos3) / 3;
        return middlePoint;
    }

    // Set a object as extra focus point at runtime
    public void setFocusObject(GameObject obj)
    {
        focusObject = obj.transform;
    }

    // Getter for distance between the players 
    public float getDistanceBetweenPlayers()
    {
        float dis;
        dis = Vector3.Distance(player1.position, player2.position);
        return dis;
    }



}
