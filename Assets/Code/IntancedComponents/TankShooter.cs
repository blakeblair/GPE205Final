using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TankShooter : Shooter
{
    private TankPawn tankPawn;
    private TankAudio tankAudio;
    private NoiseMaker noiseMaker;

    private Rigidbody rb;
    float lastShootTime;

    private void Awake()
    { 
        rb = GetComponent<Rigidbody>();
        tankAudio = GetComponent<TankAudio>();
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
        tankAudio.PlayFireSound();
        //recoil force opposite to the direction of the bullet
        rb.AddForce(-firePoint.forward * tankPawn.Parameters.knockBackForce);

    }
}
