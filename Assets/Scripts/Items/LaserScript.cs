using UnityEngine;

public class LaserScript : MonoBehaviour {
	public Transform startPoint;
	public Transform endPoint;
    public float detectionRange = 5f;
	LineRenderer laserLine;

    private ParticleSystem particleSystem;
	// Use this for initialization
	void Start () {
		laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.useWorldSpace = true;
        particleSystem = GetComponentInChildren<ParticleSystem>();
		laserLine.SetWidth (.2f, .2f);
	}
	
	// Update is called once per frame
	void Update () {
        laserLine.SetPosition(0, endPoint.position);
        RaycastHit hit;
        Vector3 fwd = endPoint.TransformDirection(Vector3.forward);
        // Debug ray cast
        Debug.DrawRay(endPoint.position, fwd * detectionRange, Color.green);

        if (Physics.Raycast(endPoint.position, fwd, out hit))
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
}
