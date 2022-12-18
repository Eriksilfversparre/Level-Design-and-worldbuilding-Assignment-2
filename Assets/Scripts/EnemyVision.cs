using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Dimension settings")]
    [SerializeField] public float radius;
    [Range(0, 360)]
    [SerializeField] public float angle;
    [SerializeField] private Vector3 offset;

    [Header("Visualization settings")]
    [SerializeField] private float meshResolution;
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private int edgeResolveIterations;
    [SerializeField] private float edgeDstThreshold;
    private Mesh viewMesh;

    [Header("Other settings")]
    [SerializeField] public GameObject player;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    float delay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        viewMesh = new Mesh
        {
            name = "View Mesh"
        };
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine(FOVRoutine());
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {

        //Check if the player is in the range
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        //If you found the player in the range
        if (rangeChecks.Length > 0)
        {
            //Get the transform of the player and the direction from you to the player
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - (transform.position + offset)).normalized;

            //Check if the player is in the cone of vision
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //Check if there's some obstacle that doesn't allow you to see the player
                if (!Physics.Raycast(transform.position + offset, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    void DrawFieldOfView()
    {
        //Precision of the drawing
        int stepCount = Mathf.RoundToInt(angle * meshResolution);
        float stepAngleSize = angle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float tempAngle = transform.eulerAngles.y - angle / 2 + stepAngleSize * i;
            ViewCastInfo edgeViewCast = ViewCast(tempAngle);

            if (i > 0)
            {
                //Check if you're going on the edge of the object or not
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - edgeViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != edgeViewCast.hit || (oldViewCast.hit && edgeViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, edgeViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(edgeViewCast.point);
            oldViewCast = edgeViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        //Check if there's an obstacle that shouldn't allow to draw the cone of view
        if (Physics.Raycast(transform.position, dir, out hit, radius, obstructionMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }

        return new ViewCastInfo(false, transform.position + dir * radius, radius, globalAngle);
    }

    private Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += this.transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
