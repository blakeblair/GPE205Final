using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISenses : MonoBehaviour
{
    public LayerMask Mask => LayerMask.GetMask("Default", "Tank");
    public AISkill Skill;
    public TankPawn Pawn;
    public Transform destination;
    public Vector3 currentWaypoint;

    public bool IsNavigatingToWaypoint => currentWaypointIndex > 0;

    [SerializeField]
    int currentWaypointIndex = -1;
    public bool traveling = false;

    [SerializeField]
    private Transform eye;
    private Collider[] SensedObjects;
    private NavMeshPath path;
    public float updateRate = 1f;

    public List<TankPawn> SensedEnemies;

    private void Awake()
    {
        currentWaypointIndex = -1;
        SensedEnemies = new List<TankPawn>();
        Pawn = GetComponent<TankPawn>();
        path = new NavMeshPath();
        SensedObjects = new Collider[16];
        lastWaypointTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastWaypointTime > updateRate)
            UpdateVision();

        //DrawDebug();
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

    public float lastWaypointTime;

    public bool GotoWaypoint(Vector3 position)
    {
        lastWaypointTime = Time.time;
        StopAllCoroutines();
        var result = NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);

        if (!result) return false;

        if (path.corners.Length > 0)
        {
            currentWaypoint = path.corners[1];
            currentWaypointIndex = 1;
        }

        StartCoroutine(Travel(currentWaypoint)); // Create the coroutine
        return true;
    }

    public float stoppingDistance = 0.5f;
    IEnumerator Travel(Vector3 position)
    {
        var distance = Vector3.Distance(position, transform.position);
        bool reachedWaypoint = distance < stoppingDistance;

        while (!reachedWaypoint) //Update Loop with a condition
        {

            traveling = true;
            NavigateTowardsWaypoint();


            distance = Vector3.Distance(position, transform.position);
            reachedWaypoint = distance < stoppingDistance;
            if (reachedWaypoint)
            {
                break; //Stop the coroutine Immedieately
            }

            yield return null; // Wait until the next frame
        }

        traveling = false;
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
            currentWaypointIndex = -1;
        }
    }

    private void NavigateTowardsWaypoint()
    {
        var direction = currentWaypoint - transform.position;
        var cross = Vector3.Cross(direction.normalized, transform.forward);
        bool facing = Facing(currentWaypoint, 90);

        //DebugPlus.LogOnScreen("Cross: " + cross + "| Facing : " + facing);

        if (cross.y < 0)
            Pawn.HullRotate(1f);
        else if (cross.y > 0)
            Pawn.HullRotate(-1f);

        if (facing && Mathf.Abs(cross.y) < 0.25f) 
            Pawn.HullMove(1f);
    }

    private void UpdateVision()
    {
        var sphereCast = Physics.OverlapSphereNonAlloc(transform.position, Skill.detectionRadius, SensedObjects, Mask, QueryTriggerInteraction.Ignore);
        SensedEnemies.Clear();
        if(sphereCast == 0)
        {
            return;
        }

        for(int i = 0; i < sphereCast; i++)
        {
            var col = SensedObjects[i];
            if (col.gameObject == gameObject) continue;
            var otherPawn = col.GetComponent<TankPawn>();
            if (otherPawn == null) continue;


            if (!Facing(col.transform.position, Skill.detectionFov)) continue;

            //DebugPlus.LogOnScreen("I am in range of " + col.name);

            var los = HasLos(col);

            if (!los) continue;

            SensedEnemies.Add(otherPawn);
        }
    }


    private bool HasLos(Collider col)
    {
        var dir = (col.transform.position - eye.transform.position).normalized;
        var ray = Physics.Raycast(eye.transform.position, dir, out var hit, Skill.detectionRadius, Mask, QueryTriggerInteraction.Ignore);
        
        if(ray)
        {
            DebugPlus.DrawLine(eye.transform.position, hit.point);
        }


        return ray && hit.collider == col;
    }

    private bool Facing(Vector3 position, float fov)
    {
        var dir = (position - transform.transform.position).normalized;
        var dot = Vector3.Dot(transform.transform.forward, dir);
        var angle = Mathf.Cos(fov * Mathf.Deg2Rad);

        return dot >= angle;
    }
}
