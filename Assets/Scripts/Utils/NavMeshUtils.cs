using UnityEngine;
using UnityEngine.AI;

namespace Mainframe.Utils
{
    public class NavMeshUtils
    {
        //Calculate the length of a path between two points.
        public static float CalculatePathLength(Vector3 pos1, Vector3 pos2)
        {
            NavMeshPath path = new NavMeshPath();
            path.ClearCorners();
            if (NavMesh.CalculatePath(pos1, pos2, 1, path))
            {
                float lng = 0.0f;

                if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
                {
                    for (int i = 1; i < path.corners.Length; ++i)
                    {
                        lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                    }
                }

                return lng;
            }
            return 0.0f;
        }
    }
    
}

