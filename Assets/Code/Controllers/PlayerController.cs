using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerController : Controller
{
    public Vector3 currentAimPoint;
    public int Lives { get; private set; } = 3;
    public Transform Cam { get; private set; }

    private TankMovement TankMovement;

    private void Awake()
    {
        pawn = GetComponent<Pawn>();
        TankMovement = GetComponent<TankMovement>();
    }

    public void TakeControl(Pawn controlledPawn, int playerNum = 0)
    {
        //Camera = playerNum == 0 ? GameManager.Instance.P1Camera.transform : GameManager.Instance.p2Camera.transform;

        pawn = controlledPawn;
        Cam.SetParent(pawn.cameraMount);
        Cam.localPosition = Vector3.zero;
        Cam.localRotation = Quaternion.identity;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void Respawn()
    {
        Lives--;

        if(Lives == 0)
        {
            pawn.Movement.enabled = false;
            GameManager.Instance.GameOver(this);
            return;
        }

        GameManager.Instance.Respawn(pawn);
        pawn.Health.Heal(100);
    }

    public override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        if (Cam != null)
            Cam.transform.parent = null;
    }

    private void OnDeath()
    {
        Respawn();
    }

    public override void Update()
    {
        if (pawn == null) return;
        ProcessInputs();
        
        base.Update();
    }

    public override void ProcessInputs()
    {
        var move = GameManager.Controls.FindAction("Move").ReadValue<Vector2>();
        var look = GameManager.Controls.FindAction("Look").ReadValue<Vector2>();
        
        if (Dead)
        {
            move = Vector2.zero;
            look = Vector2.zero;
        }

        pawn.HullRotate(move.x);
        pawn.HullMove(move.y);

        pawn.TurretRotate(look.x);
        pawn.TurretPitch(look.y);

        ShootInput();
    }

    private void ShootInput()
    {
        var shoot = GameManager.Controls.FindAction("Shoot").WasPressedThisFrame();

        if(shoot)
        {
            pawn.Shoot();
        }
    }
}
