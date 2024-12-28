using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    private TankPawn tankPawn;
    private NoiseMaker noiseMaker;

    private void Awake()
    { 
        noiseMaker = GetComponent<NoiseMaker>();
        tankPawn = GetComponent<TankPawn>();
    }

    public override void Shoot()
    {
        Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        newBullet.InitBullet(tankPawn, firePoint.forward);
        noiseMaker.shootingVolume = noiseMaker.shootingNoiseMultiplier;
    }
}
