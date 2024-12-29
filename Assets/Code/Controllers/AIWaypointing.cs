using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIWaypointing : MonoBehaviour
{
    public float lastWaypointTime;
    public float stoppingDistance = 0.5f;
    private NavMeshPath path;
    public Vector3 currentWaypoint;
    
    public bool IsNavigatingToWaypoint => currentWaypointIndex > 0;

    [SerializeField]
    int currentWaypointIndex = -1;

    TankPawn Pawn;
    AISenses Senses;

    private void Awake()
    {
        lastWaypointTime = Time.time;
        path = new NavMeshPath();
        currentWaypointIndex = -1;
    }

    private void Start()
    {
        Senses = GetComponent<AISenses>();
        Pawn = GetComponent<TankPawn>();
    }

    private void Update()
    {
        DrawDebug();
    }

    private void DrawDebug()
    {
        if (path == null) return;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            DebugPlus.DrawLine(path.corners[i], path.corners[i + 1]);
        }

        DebugPlus.DrawSphere(currentWaypoint, 1f).color = Color.blue;
    }


    public bool SetDestination(Vector3 position)
    {
        lastWaypointTime = Time.time;
        StopAllCoroutines();
        var result = NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);

        if (!result) return false;

        if (path.corners.Length > 1)
        {
            currentWaypoint = path.corners[1];
            currentWaypointIndex = 1;
        }

        StartCoroutine(Travel(currentWaypoint)); // Create the coroutine
        return true;
    }

    public void Stop()
    {
        StopAllCoroutines();
        currentWaypointIndex = -1;
    }

    IEnumerator Travel(Vector3 position)
    {
        var distance = Vector3.Distance(position, transform.position);
        bool reachedWaypoint = distance < stoppingDistance;

        while (!reachedWaypoint) //Update Loop with a condition
        {
            MoveTowardsPositionIfFacing(currentWaypoint);
            FaceTowardsPosition(currentWaypoint);

            distance = Vector3.Distance(position, transform.position);
            reachedWaypoint = distance < stoppingDistance;
            if (reachedWaypoint)
            {
                break; //Stop the coroutine Immedieately
            }

            yield return null; // Wait until the next frame
        }

        GotoNextWaypoint();

    }

    private void GotoNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex < path.corners.Length)
        {
            //DebugPlus.LogOnScreen("Next is " + currentWaypointIndex).duration = 1.0f;
            currentWaypoint = path.corners[currentWaypointIndex];
            StartCoroutine(Travel(currentWaypoint));
        }
        else
        {
            Stop();
        }
    }

    public void MoveTowardsPositionIfFacing(Vector3 position)
    {
        var direction = position - transform.position;
        var cross = Vector3.Cross(direction.normalized, transform.forward);
        bool facing = Senses.Facing(position, 90);

        if (facing && Mathf.Abs(cross.y) < 0.25f)
            Pawn.HullMove(1f);
    }

    public void FaceTowardsPosition(Vector3 position)
    {
        var direction = position - transform.position;
        var cross = Vector3.Cross(direction.normalized, transform.forward);

        if (cross.y < 0)
            Pawn.HullRotate(1f);
        else if (cross.y > 0)
            Pawn.HullRotate(-1f);
    }


}
