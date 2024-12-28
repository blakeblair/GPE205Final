using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISenses : MonoBehaviour
{
    public LayerMask Mask => LayerMask.GetMask("Default", "Tank");
    public AISkill Skill;
    public TankPawn Pawn;

    [SerializeField]
    private Transform eye;
    private Collider[] sensedObjects;
    public float updateRate = 1f;

    private float lastUpdateTime;
    public List<TankPawn> SensedEnemies;

    private void Awake()
    {
        SensedEnemies = new List<TankPawn>();
        Pawn = GetComponent<TankPawn>();
        sensedObjects = new Collider[16];
    }

    private void Update()
    {
        if (Time.time - lastUpdateTime > updateRate)
            UpdateVision();

    }

    private void UpdateVision()
    {
        var sphereCast = Physics.OverlapSphereNonAlloc(transform.position, Skill.detectionRadius, sensedObjects, Mask, QueryTriggerInteraction.Ignore);
        SensedEnemies.Clear();
        if(sphereCast == 0)
        {
            return;
        }

        for(int i = 0; i < sphereCast; i++)
        {
            var col = sensedObjects[i];
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


    public bool HasLos(Collider col)
    {
        var dir = (col.transform.position - eye.transform.position).normalized;
        var ray = Physics.Raycast(eye.transform.position, dir, out var hit, Skill.detectionRadius, Mask, QueryTriggerInteraction.Ignore);
        
        if(ray)
        {
            DebugPlus.DrawLine(eye.transform.position, hit.point);
        }


        return ray && hit.collider == col;
    }

    public bool Facing(Vector3 position, float fov)
    {
        var dir = (position - transform.transform.position).normalized;
        var dot = Vector3.Dot(transform.transform.forward, dir);
        var angle = Mathf.Cos(fov * Mathf.Deg2Rad);

        return dot >= angle;
    }
}
