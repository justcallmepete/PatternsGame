using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Same principle as the Light Area code. Guard raycasts multiple times infront of him to detect walls and players. These points will be made into one mesh. This represents the vision.
 */ 

public class GuardVision : MonoBehaviour {

    [Range(0, 20)]
    public float viewRadius;
    [Range(0, 20)]
    public float viewRadiusInLight; 
    [Range(0, 360)]
    public float viewAngle;

    int obstacleLayersIndex;
    int playerLayerIndex;
    int lightLayerMask;
    LayerMask playerLayerMask;
    LayerMask obstacleLayerMask;

    public float meshResolution;

    public MeshFilter viewMeshFiler;
    public MeshFilter viewMeshFilerInLight;
    Mesh viewMesh;
    Mesh viewMeshInLight;

    [SerializeField]
    int edgeCheckIteration;
    [SerializeField]
    float edgeThresholdDistance = 0.5f;
        
    List<Transform> playersInSight = new List<Transform>();

    [SerializeField]
    float checkPlayerInSightDelay = 0.5f;
    // Use this for initialization

    [ExecuteInEditMode]
    void OnValidate()
    {
        if (viewRadiusInLight < viewRadius) viewRadiusInLight = viewRadius;
    }
    void Start () {
        viewMesh = new Mesh();
        viewMesh.name = "viewMesh";
        viewMeshFiler.mesh = viewMesh;
        viewMeshInLight = new Mesh();
        viewMeshInLight.name = "viewMeshInLight";
        viewMeshFilerInLight.mesh = viewMeshInLight;

        obstacleLayersIndex = LayerMask.NameToLayer("Wall");
        playerLayerIndex = LayerMask.NameToLayer("Player");
        obstacleLayerMask = 1 << obstacleLayersIndex;
        playerLayerMask = 1 << playerLayerIndex;

        lightLayerMask = (1 << obstacleLayersIndex) | (1 << playerLayerIndex);

        StartCoroutine("CheckTargetWithDelay", checkPlayerInSightDelay);
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator CheckTargetWithDelay(float pDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(pDelay);
            CheckIfPlayerInSight();
        }
    }

    void CheckIfPlayerInSight()
    {
        playersInSight.Clear();
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadiusInLight, playerLayerMask);
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.GetComponent<MainPlayer>()) continue;
            //Check if in sightAngle
            Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                //Check distance to target 
                float distToTarget = Vector3.Distance(transform.position, targets[i].transform.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleLayerMask))
                {
                    //Draw line if in sight
                    Debug.DrawLine(transform.position, transform.position + dirToTarget * distToTarget, Color.red, 2f);
                    playersInSight.Add(targets[i].transform);

                }

            }
        }

        GameObject closestPlayer = null;
        if(playersInSight.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < playersInSight.Count; i++)
            {
                float distToTarget = Vector3.Distance(transform.position, playersInSight[i].transform.position);
                if (distToTarget < closestDistance)
                {
                    float sightDistance = viewRadius;
                    if (playersInSight[i].GetComponent<MainPlayer>().IsStandingInLight)
                    {
                        sightDistance = viewRadiusInLight;
                    }
                    if (distToTarget <= sightDistance)
                    {
                        closestDistance = distToTarget;
                        closestPlayer = playersInSight[i].gameObject;
                    }
                }
            }
        }
        
        if (closestPlayer != null)
        {
            if (GetComponentInParent<GuardStateMachine>().TargetPlayer == null)
            {
                SpotPlayer(closestPlayer);
            }
        }
        else
        {
            if (GetComponentInParent<GuardStateMachine>().TargetPlayer != null) LosePlayer(GetComponentInParent<GuardStateMachine>().TargetPlayer.transform.position);
        }
    }

    private void LosePlayer(Vector3 position)
    {
        GetComponentInParent<GuardStateMachine>().PlayerLost(position);
    }

    private void SpotPlayer(GameObject player)
    {
        Debug.Log("spotting player");
        GetComponentInParent<GuardStateMachine>().Alert(player);
    }

    /// <summary>
    /// This method will visualize the light. First it will Viewcast multiple times to get get the hitpoints. From these hitpoints this method will draw the mesh.
    /// </summary>
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        //This part will raycast multiple times to get the hitpoints. 
        List<Vector3> viewPoints = new List<Vector3>();
        List<Vector3> viewPointsInLight = new List<Vector3>();
        ViewCastInfo oldViewCast = ViewCast(transform.position, transform.eulerAngles.y - viewAngle / 2, viewRadius);
        viewPoints.Add(oldViewCast.point);
        if (!oldViewCast.hit)
        {
            ViewCastInfo lightViewCast = ViewCast(oldViewCast.point, transform.eulerAngles.y - viewAngle / 2, viewRadiusInLight - viewRadius);
            viewPointsInLight.Add(lightViewCast.point);
        }
        else
        {
            viewPointsInLight.Add(oldViewCast.point);
        }

        for (int i = 1; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(transform.position, angle, viewRadius);
            bool edgeDistanceThresholdExceed = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeThresholdDistance;
            if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceed)) // Check if there is an edge inbetween raycasts. 
            {
                //Get edge cast
                EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                if (edge.pointA != Vector3.zero) viewPoints.Add(edge.pointA);
                if (edge.pointB != Vector3.zero) viewPoints.Add(edge.pointB);
            }
            viewPoints.Add(newViewCast.point);

            // Seccond layer (in light area)
            if(!newViewCast.hit)
            {
                ViewCastInfo lightViewCast = ViewCast(newViewCast.point, angle, viewRadiusInLight - viewRadius);
                viewPointsInLight.Add(lightViewCast.point);
            }
            else
            {
                viewPointsInLight.Add(newViewCast.point);
            }
            oldViewCast = newViewCast;
        }
        int vertexCount = viewPoints.Count + 1; // +1 is transform position
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        int vertexCountInLight = viewPointsInLight.Count;

        //Mesh will be a child of, so need to calculate in world space
        vertices[0] = Vector3.zero;

        //This part will set the vertices and trinagles for the viewmesh.
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            vertices[i + 1].y = 0.1f;
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        CreateMesh(viewMesh, vertices, triangles);

        if (vertexCountInLight > 2)
        {
            Vector3[] verticesInLight = new Vector3[vertexCountInLight + vertexCount - 1];
            int[] trianglesInLight = new int[(vertexCountInLight) * 3];
            for (int i = 0; i < vertexCountInLight; i += 2)
            {
                verticesInLight[i] = transform.InverseTransformPoint(viewPoints[i]);            // 0, 2, 4, 6, 8
                verticesInLight[i+1] = transform.InverseTransformPoint(viewPointsInLight[i]);   // 1, 3, 5, 7
                verticesInLight[i].y = 0.1f;
                verticesInLight[i+1].y = 0.1f;
                if (i < vertexCountInLight - 2)
                {
                    trianglesInLight[i * 3] = i;                // 0 [0]
                    trianglesInLight[i * 3 + 1] = i+1;          // 1 [1]
                    trianglesInLight[i * 3 + 2] = i + 2;        // 2 [2]

                    trianglesInLight[(i + 1) * 3] = i + 1;      // 1 [3]
                    trianglesInLight[(i + 1) * 3 + 1] = i + 3;  // 3 [4]
                    trianglesInLight[(i + 1) * 3 + 2] = i + 2;  // 2 [5]
                }
            }
            CreateMesh(viewMeshInLight, verticesInLight, trianglesInLight);
        }
    }

    /// <summary>
    /// This method will be called if there is an edge to be found. It will iterate multiple times to get as close to the edge as possible. The output will be Edgeinfo.
    /// </summary>
    /// <param name="minViewCast"></param>
    /// <param name="maxViewCast"></param>
    /// <returns> EdgeInfo </returns>
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeCheckIteration; i++)
        {
            float midAngle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(transform.position, midAngle, viewRadius);

            bool edgeDistanceThresholdExceed = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeThresholdDistance;
            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceed)
            {
                minAngle = midAngle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = midAngle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);

    }

    void CreateMesh(Mesh pMesh, Vector3[] pVertices, int[] pTriangles)
    {
        pMesh.Clear();
        pMesh.vertices = pVertices;
        pMesh.triangles = pTriangles;
        pMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(Vector3 fromPosition, float pGlobalAngle, float distance)
    {
        Vector3 dir = DirFromAngle(pGlobalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(fromPosition, dir, out hit, distance, lightLayerMask))
        {
            if (hit.collider.gameObject.GetComponent<PlayerLight>())
            {
                hit.collider.gameObject.GetComponent<PlayerLight>().isInLight = true;
            }
            return new ViewCastInfo(true, hit.point, hit.distance, pGlobalAngle);
        }
        return new ViewCastInfo(false, fromPosition + dir * distance, distance, pGlobalAngle);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _dist;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        //One of these is the closest ON the obstacle, the one closest OFF the obstacle
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pPointA, Vector3 pPointB)
        {
            pointA = pPointA;
            pointB = pPointB;
        }
    }
}
