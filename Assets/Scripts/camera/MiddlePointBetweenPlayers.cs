using UnityEngine;

/*

    Class is used to define the middle point between the players.

*/
public class MiddlePointBetweenPlayers : MonoBehaviour {

    [SerializeField]
    GameObject player1;
    [SerializeField]
    GameObject player2;

    Transform posP1;
    Transform posP2;
    Vector3 MiddlePoint;

    [Header ("The delay in the response of the position of the camera (lerp) (higher is less delay)")]
    [SerializeField]
    [Range(0, 5)]
    float speed = 2.5f;


	void Start () {

        posP1 = player1.GetComponent<Transform>();
        posP2 = player2.GetComponent<Transform>();
       
	}
	
	void Update () {

        // Adding a lerp effect while defining the middle.
        float step = speed * Time.deltaTime;
        MiddlePoint = (posP1.position + posP2.position) / 2;
        transform.position = Vector3.Lerp(transform.position, MiddlePoint, step);

   
	}

    // Getter for distance between the players 
    public float getDistanceBetweenPlayers()
    {
        float dis;
        dis = Vector3.Distance(posP1.position, posP2.position);
        return dis;
    }



}
