using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArea : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask obstacleLayers;
    int playerLayerIndex;
    int lightLayerMask; 
    [HideInInspector]
    public List<Transform> targetsInSight = new List<Transform>();

    public float meshResolution;

    public MeshFilter viewMeshFiler;
    Mesh viewMesh;

    [SerializeField]
    int edgeCheckIteration;
    [SerializeField]
    float edgeThresholdDistance = 0.5f;
    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "viewMesh";
        viewMeshFiler.mesh = viewMesh;


        playerLayerIndex = LayerMask.NameToLayer("Player");
        lightLayerMask = (1 << playerLayerIndex) | (1 << obstacleLayers);

        Debug.Log(lightLayerMask);

        StartCoroutine("CheckTargetWithDelay", 2f);
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
            CheckTargetInSight();
        }
    }

    void CheckTargetInSight()
    {

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
        ViewCastInfo oldViewCast = ViewCast(transform.eulerAngles.y - viewAngle / 2);
        viewPoints.Add(oldViewCast.point);
        for (int i = 1; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDistanceThresholdExceed = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeThresholdDistance;
            if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceed)) // Check if there is an edge inbetween raycasts. 
            {
                //Get edge cast
                EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                if (edge.pointA != Vector3.zero) viewPoints.Add(edge.pointA);
                if (edge.pointB != Vector3.zero) viewPoints.Add(edge.pointB);
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }
        int vertexCount = viewPoints.Count + 1; // +1 is transform position
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

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
            ViewCastInfo newViewCast = ViewCast(midAngle);

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

    ViewCastInfo ViewCast(float pGlobalAngle)
    {
        Vector3 dir = DirFromAngle(pGlobalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, lightLayerMask))
        {
            if (hit.collider.gameObject.GetComponent<PlayerLight>())
            {
                hit.collider.gameObject.GetComponent<PlayerLight>().isInLight = true;
            }
            return new ViewCastInfo(true, hit.point, hit.distance, pGlobalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, pGlobalAngle);
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
