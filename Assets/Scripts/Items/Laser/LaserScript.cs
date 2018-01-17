using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Transform startPoint;
    public float detectionRange = 5f;

    private bool raycastOn = true;

    private LineRenderer laserLine;
    private ParticleSystem particleSystem;
    private LaserMovement laserMovement;

    [SerializeField]
    bool isOffOnStart = false;

    void Start()
    {
        laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.useWorldSpace = true;

        particleSystem = GetComponentInChildren<ParticleSystem>();
        laserLine.SetWidth(.2f, .2f);

        laserMovement = GetComponent<LaserMovement>();

        if (isOffOnStart) ToggleLaser();
    }

    void Update()
    {
        if (!raycastOn)
        {
            return;
        }

        laserLine.SetPosition(0, startPoint.position);

        RaycastHit hit;
        Vector3 fwd = startPoint.TransformDirection(Vector3.forward);

        if (Physics.Raycast(startPoint.position, fwd, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                MainPlayer mainPlayer = hit.transform.GetComponent<MainPlayer>();
                if (!mainPlayer.IsDead())
                {
                    mainPlayer.GetHit();
                }
            }

            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, fwd * Mathf.Infinity);
        }

        particleSystem.startLifetime = hit.distance / particleSystem.startSpeed;
    }

    // Turn on or off the laser.
    public void ToggleLaser()
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        if (!raycastOn)
        {
            raycastOn = true;
            laserLine.enabled = true;
            emission.enabled = true;
            ToggleMovement(true);
        }
        else
        {
            raycastOn = false;
            laserLine.enabled = false;
            emission.enabled = false;
            ToggleMovement(false);
        }
    }

    // Turn on or off the movement of the laser.
    public void ToggleMovement(bool turnMovementOn)
    {
        laserMovement.isMoveable = turnMovementOn;
    }
}
