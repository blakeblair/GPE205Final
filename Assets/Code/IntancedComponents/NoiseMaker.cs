using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    public float volumeDistance;

    public float movingVolume { get; set; }
    public float rotatingVolume { get; set; }
    public float shootingVolume { get; set; }


    //can be tweaked so rotating is quieter than moving
    public float moveNoiseMultiplier = 5;
    public float rotateNoiseMultiplier = 5;
    public float shootingNoiseMultiplier = 10f;

    public TankMovement tankMovement;

    private void Awake()
    {
        tankMovement = GetComponent<TankMovement>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, volumeDistance);
    }

    private void FixedUpdate()
    {
        movingVolume = Mathf.Abs(tankMovement.MovementInput.y) * moveNoiseMultiplier;
        rotatingVolume = Mathf.Abs(tankMovement.MovementInput.x) * rotateNoiseMultiplier;
        shootingVolume = Mathf.Clamp(shootingVolume-0.1f, 0, shootingNoiseMultiplier);
    }

    internal void MakeNoise()
    {
        float noiseVolume = movingVolume + rotatingVolume + shootingVolume;
        volumeDistance = noiseVolume;
    }
}