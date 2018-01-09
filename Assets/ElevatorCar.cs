using System.Collections;
using UnityEngine;

/*
 * This script will check if any player is in this box collider. A trigger box collider is required.
 * The next scene in the build setting will be loaded after this 
 */
[RequireComponent(typeof(BoxCollider))]
public class ElevatorCar : MonoBehaviour
{
    [Tooltip("Delay in seconds for loading next scene.")]
    public float loadSceneDelay = 3;

    private int requiredAmountOfPlayers;
    private int nPlayer = 0;
    private Elevator elevator;

    void Awake()
    {
        requiredAmountOfPlayers = GameManager.Instance.Players.Length;
        elevator = GetComponentInParent<Elevator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nPlayer++;

            if (nPlayer == requiredAmountOfPlayers)
            {
                DepartElevator();
                SetDepartElevator(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nPlayer--;
            SetDepartElevator(false);
        }
    }

    private void DepartElevator()
    {
        elevator.DepartElevator();
    }

    public void SetDepartElevator(bool canDepart)
    {
        elevator.Anim.SetBool("doDepart", canDepart);
    }
}
