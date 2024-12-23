using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [field:SerializeField]
    public int Lives { get; private set; } = 3;
    public int Score { get; private set; }

    public int PlayerNumber;
    public Transform Camera { get; private set; }
    public void TakeControl(Pawn controlledPawn, int playerNum = 0)
    {
        //Camera = playerNum == 0 ? GameManager.Instance.P1Camera.transform : GameManager.Instance.p2Camera.transform;

        pawn = controlledPawn;
        Camera.SetParent(pawn.cameraMount);
        Camera.localPosition = Vector3.zero;
        Camera.localRotation = Quaternion.identity;
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

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //pawn.Health.DeathEvent += OnDeath;
    }

    private void OnDestroy()
    {
        if (Camera != null)
            Camera.transform.parent = null;

        //pawn.Health.DeathEvent -= OnDeath;
    }

    private void OnDeath()
    {
        Respawn();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (pawn == null) return;
        ProcessInputs();
        
        base.Update();
    }

    public override void ProcessInputs()
    {
        //get input from unity input system for hull forward/backward and rotation and turret rotation
        
    }
}
