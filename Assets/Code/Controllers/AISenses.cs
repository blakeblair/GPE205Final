using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISenses : MonoBehaviour
{
    public LayerMask Mask => LayerMask.GetMask("Tank");
    public AISkill Skill;
    public TankPawn Pawn;
    public Shooter Shooter;

    private Transform eye;
    private Collider[] sensedObjects;
    public float updateRate = 1f;

    private float lastUpdateTime;
    public List<TankPawn> SensedEnemies;
    public List<TankPawn> HeardEnemies;

    private void Awake()
    {
        Shooter = GetComponent<Shooter>();
        SensedEnemies = new List<TankPawn>();
        HeardEnemies = new List<TankPawn>();
        Pawn = GetComponent<TankPawn>();
        sensedObjects = new Collider[16];

        eye = Shooter.firePoint;


    }

    private void Update()
    {
        UpdateVision();

        UpdateHearing();

    }

    public bool CanHear(NoiseMaker noise)
    {
        if (Vector3.Distance(noise.transform.position, transform.position) < noise.volumeDistance + Skill.hearingRadius)
        {
            return true;
        }
        return false;
    }

    private void UpdateHearing()
    {
        HeardEnemies.Clear();
        foreach(var tank in GameManager.Instance.AllPawns)
        {
            if (tank == null) continue;
            if (tank == Pawn) continue;
            if (!tank.GetComponent<PlayerController>()) continue;

            var heard = CanHear(tank.NoiseMaker);

            if (heard)
            {
                HeardEnemies.Add(tank);
            }
        }
    }

    private void UpdateVision()
    {
        lastUpdateTime = Time.time;
        var sphereCast = Physics.OverlapSphereNonAlloc(transform.position, Skill.sightRadius, sensedObjects, Mask, QueryTriggerInteraction.Ignore);
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

            if (!otherPawn.GetComponent<PlayerController>()) continue;

            if (!Facing(col.transform.position, Skill.detectionFov)) continue;

            //DebugPlus.LogOnScreen("I am in range of " + col.name);

            var los = HasLos(col);

            if (!los) continue;

            SensedEnemies.Add(otherPawn);
        }
    }


    public bool HasLos(Collider col)
    {
        var start = new Vector3(transform.position.x, eye.position.y, transform.position.z);
        var dir = (col.transform.position - start).normalized;
        var ray = Physics.Raycast(start, dir, out var hit, Skill.sightRadius, Mask, QueryTriggerInteraction.Ignore);
        
        //if(ray)
        //{
        //    DebugPlus.DrawLine(start, hit.point);
        //}


        return ray && hit.collider == col;
    }

    public bool HasClearShot(Collider col)
    {
        var dir = eye.transform.forward;//(col.transform.position - eye.transform.position).normalized;
        var ray = Physics.Raycast(eye.transform.position, dir, out var hit, Skill.sightRadius, Mask, QueryTriggerInteraction.Ignore);

        //if (ray)
        //{
        //    DebugPlus.DrawLine(eye.transform.position, hit.point).color = Color.magenta;
        //}


        return ray && hit.collider == col;
    }

    //If transform.forward is facing fov degress of position
    public bool Facing(Vector3 position, float fov)
    {
        var dir = (position - transform.transform.position).normalized;
        var dot = Vector3.Dot(transform.transform.forward, dir);
        var angle = Mathf.Cos(fov * Mathf.Deg2Rad);

        return dot >= angle;
    }
}
