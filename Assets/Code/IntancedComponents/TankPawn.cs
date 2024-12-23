using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    public float speed = 10;
    public float rotationSpeed = 10;
    public float turretRotationSpeed = 10;
    public float turretRotationLimit = 45;
    public GameObject turret;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10;
    public float fireRate = 1;
    private float nextFireTime = 0;


    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(float verticalInput)
    {
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);
    }

    public override void Rotate(float horizontalInput)
    {
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
    }

    public override void MakeNoise()
    {
        
    }

    //public override void ActivatePowerup(PowerupType powerup)
    //{
        
    //}

    public void RotateTurret(float horizontalInput)
    {
        turret.transform.Rotate(Vector3.up * horizontalInput * turretRotationSpeed * Time.deltaTime);
        float angle = turret.transform.localEulerAngles.y;
        if (angle > 180)
        {
            angle -= 360;
        }
        angle = Mathf.Clamp(angle, -turretRotationLimit, turretRotationLimit);
        turret.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

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
