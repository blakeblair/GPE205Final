using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public Movement Movement;
    public Shooter Shooter;
    public Health Health;

    public float hullMoveSpeed;
    public float hullRotateSpeed;
    public float attackRate;
    public Transform cameraMount;
    
    
    public abstract void Shoot();
    
    public abstract void HullMove(float verticalInput);

    public abstract void HullRotate(float horizontalInput);
    
    public abstract void TurretRotate(float horizontalInput);
    
    public abstract void MakeNoise();
    
    //public abstract void ActivatePowerup(PowerupType powerup);
    
}
