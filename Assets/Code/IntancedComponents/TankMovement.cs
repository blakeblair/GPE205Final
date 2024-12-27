using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    public Transform pivot;
    public Transform turret;
    public Vector2 MovementInput;
    public Vector2 TurretInput;

    public float moveSpeed = 10;
    public float rotationSpeed = 10;
    public float cameraSpeed = 10;
    public float turretRotationSpeed = 10;
    public float turretRotationLimit = 45;

    public override void HullMove(float vertical)
    {
        this.MovementInput.y = vertical;
    }

    public override void HullRotate(float horizontal)
    {
        this.MovementInput.x = horizontal;
    }


    public override void TurretRotate(float horizontal)
    {
        this.TurretInput.x = horizontal;
    }
    public override void TurretPitch(float vertical)
    {
        this.TurretInput.y = vertical;
    }

    private void Update()
    {
        transform.position = transform.position + transform.forward * MovementInput.y * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * MovementInput.x * rotationSpeed * Time.deltaTime);
        RotatePivot();
        VerticalRotation();
    }

    private void LateUpdate()
    {
        turret.rotation = Quaternion.RotateTowards(turret.rotation, pivot.rotation, turretRotationSpeed * Time.deltaTime);
    }

    public void RotatePivot()
    {
        pivot.transform.Rotate(Vector3.up * TurretInput.x * cameraSpeed * Time.deltaTime);

        //float angle = turret.transform.localEulerAngles.y;
        //if (angle > 180)
        //{
        //    angle -= 360;
        //}
        //angle = Mathf.Clamp(angle, -turretRotationLimit, turretRotationLimit);
        //turret.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    private void VerticalRotation()
    {

        pivot.transform.rotation *= Quaternion.AngleAxis(TurretInput.y * cameraSpeed * Time.deltaTime, Vector3.right);

        var angles = pivot.transform.localEulerAngles;
        angles.z = 0;

        var x = angles.x;

        if (x > 180 && x < 340)
        {
            x = 340;
        }
        else if (x < 180 && x > 40)
        {
            x = 40;
        }

        angles.x = x;
        pivot.transform.localEulerAngles = angles;

    }
}
