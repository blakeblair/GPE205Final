using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Health))]
//[RequireComponent(typeof(NoiseMaker))]

public class TankPawn : Pawn
{
    public TankParameters Parameters = null;
    public NoiseMaker NoiseMaker;
    private void Awake()
    {
        NoiseMaker = GetComponent<NoiseMaker>();
        Health = GetComponent<Health>();
        Shooter = GetComponent<TankShooter>();
        Movement = GetComponent<TankMovement>();
    }

    private void Start()
    {
        Health.CurrentHealth = Parameters.MaxHealth;
        Health.MaxHealth = Parameters.MaxHealth;
    }

    private void OnDestroy()
    {
        if (Parameters)
            Destroy(Parameters);
        Parameters = null;
    }

    public override void Shoot()
    {
        Shooter.Shoot();
    }

    public override void HullMove(float verticalInput)
    {
        Movement.HullMove(verticalInput);
    }

    public override void HullRotate(float horizontalInput)
    {
        Movement.HullRotate(horizontalInput);
    }
    
    public override void TurretRotate(float horizontalInput)
    {
        Movement.TurretRotate(horizontalInput);
    }

    public override void TurretPitch(float verticalInput)
    {
        Movement.TurretPitch(verticalInput);
    }


    public override void MakeNoise()
    {
        
    }

    public void SetParameters(TankParameters baseTankParameters)
    {
        Parameters = Instantiate(baseTankParameters);
    }

    //public override void ActivatePowerup(PowerupType powerup)
    //{

    //}

}
