using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    public Transform pivot;
    public Transform turret;
    public Vector2 MovementInput;
    public Vector2 TurretInput;

    public TankPawn pawn;


    private void Awake()
    {
        pawn = GetComponent<TankPawn>();
    }

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
    private void LateUpdate()
    {
        transform.position = transform.position + transform.forward * MovementInput.y * pawn.Parameters.hullMoveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * MovementInput.x * pawn.Parameters.hullRotationSpeed * Time.deltaTime);

        RotatePivot();
        VerticalRotation();
        turret.rotation = Quaternion.RotateTowards(turret.rotation, pivot.rotation, pawn.Parameters.turretRotationSpeed * Time.deltaTime);

        ClearInput();
    }

    private void ClearInput()
    {
        TurretInput = new Vector2();
        MovementInput = new Vector2();
    }

    public void RotatePivot()
    {
        pivot.transform.Rotate(Vector3.up * TurretInput.x * pawn.Parameters.cameraSpeed * Time.deltaTime);

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

        pivot.transform.rotation *= Quaternion.AngleAxis(TurretInput.y * pawn.Parameters.cameraSpeed * Time.deltaTime, Vector3.right);

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
