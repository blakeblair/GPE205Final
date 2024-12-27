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

    public GameObject turret;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10;
    public float fireRate = 1;
    private float nextFireTime = 0;

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


    public void Fire()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        }
    }
}
