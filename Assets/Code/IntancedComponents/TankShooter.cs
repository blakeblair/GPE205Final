using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TankShooter : Shooter
{
    private TankPawn tankPawn;
    private NoiseMaker noiseMaker;

    float lastShootTime;

    private void Awake()
    { 
        lastShootTime = Time.time;
        noiseMaker = GetComponent<NoiseMaker>();
        tankPawn = GetComponent<TankPawn>();
    }


    public override void Shoot()
    {
        if (Time.time - lastShootTime < tankPawn.Parameters.ShootFrequency) return;

        Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        newBullet.InitBullet(tankPawn, firePoint.forward);
        noiseMaker.shootingVolume = noiseMaker.shootingNoiseMultiplier;
        lastShootTime = Time.time;

        GetComponent<Rigidbody>().AddForce(-transform.forward * tankPawn.Parameters.knockBackForce);

    }
}
