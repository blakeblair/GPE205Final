using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    public Transform pivot;
    public Transform turret;
    public float verticalInput;
    public float horizontalInput;
    public float turretInput;

    public float moveSpeed = 10;
    public float rotationSpeed = 10;
    public float cameraSpeed = 10;
    public float turretRotationSpeed = 10;
    public float turretRotationLimit = 45;

    public override void HullMove(float vertical)
    {
        this.verticalInput = vertical;
        transform.position = transform.position +transform.forward * vertical * moveSpeed * Time.deltaTime;
    }

    public override void HullRotate(float horizontal)
    {
        this.horizontalInput = horizontal;
        transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);
    }

    public override void TurretRotate(float horizontal)
    {
        this.turretInput = horizontal;
        RotateTurret(horizontal);
    }

    private void LateUpdate()
    {
        turret.rotation = Quaternion.RotateTowards(turret.rotation, pivot.rotation, turretRotationSpeed * Time.deltaTime);
    }

    public void RotateTurret(float horizontalInput)
    {
        pivot.transform.Rotate(Vector3.up * horizontalInput * cameraSpeed * Time.deltaTime);

        //float angle = turret.transform.localEulerAngles.y;
        //if (angle > 180)
        //{
        //    angle -= 360;
        //}
        //angle = Mathf.Clamp(angle, -turretRotationLimit, turretRotationLimit);
        //turret.transform.localEulerAngles = new Vector3(0, angle, 0);
    }
}
