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

    private void Awake()
    {
        Health = GetComponent<Health>();
        Shooter = GetComponent<TankShooter>();
        Movement = GetComponent<TankMovement>();

        //TankAudio = GetComponent<TankAudio>();
        //NoiseMaker = GetComponent<NoiseMaker>();
        //PowerupManager = GetComponent<PowerupManager>();
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

    //public override void ActivatePowerup(PowerupType powerup)
    //{
        
    //}

}
