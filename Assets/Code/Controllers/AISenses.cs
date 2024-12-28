using System;
using UnityEngine;

public class AISenses : MonoBehaviour
{
    public AISkill Skill;

    Collider[] results;
    public LayerMask Mask => LayerMask.GetMask("Default", "Tank");

    [SerializeField]
    private Transform eye;

    private void Awake()
    {
        results = new Collider[16];
    }

    private void Update()
    {
        UpdateVision();
    }


    private void UpdateVision()
    {
        var sphereCast = Physics.OverlapSphereNonAlloc(transform.position, Skill.detectionRadius, results, Mask, QueryTriggerInteraction.Ignore);

        if(sphereCast == 0)
        {
            return;
        }

        for(int i = 0; i < sphereCast; i++)
        {
            var col = results[i];
            if (col.gameObject == gameObject) continue;
            if (col.GetComponent<TankPawn>() == null) continue;


            if (!InRange(col)) continue;

            DebugPlus.LogOnScreen("I am in range of " + col.name);

            var los = HasLos(col);

            if (!los) continue;

            DebugPlus.LogOnScreen("I see " + col.name);

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

    private bool InRange(Collider col)
    {
        var dir = (col.transform.position - eye.transform.position).normalized;

        var dot = Vector3.Dot(eye.transform.forward, dir);

        var angle = Mathf.Cos(Skill.detectionFov * Mathf.Deg2Rad);

        DebugPlus.LogOnScreen(dot);
        return dot >= angle;
    }
}
