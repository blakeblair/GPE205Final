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

    private void Update()
    {
        movingVolume = Mathf.Abs(tankMovement.MovementInput.magnitude) * moveNoiseMultiplier;
        rotatingVolume = Mathf.Abs(tankMovement.TurretInput.magnitude) * rotateNoiseMultiplier;
        shootingVolume = Mathf.Clamp(shootingVolume-0.1f, 0, shootingNoiseMultiplier);

        float noiseVolume = movingVolume + rotatingVolume + shootingVolume;
        volumeDistance = Mathf.Lerp(volumeDistance,noiseVolume,Time.deltaTime);

        //DebugPlus.LogOnScreen(tankMovement.MovementInput);
        //DebugPlus.LogOnScreen(tankMovement.TurretInput);
        //DebugPlus.LogOnScreen(volumeDistance);
    }
}