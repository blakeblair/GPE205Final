using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public Vector3 currentAimPoint;
    
    [field:SerializeField]
    public int Lives { get; private set; } = 3;
    public int Score { get; private set; }

    public int PlayerNumber;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private Transform aimCursor;
    [SerializeField] private Transform tankturret;
    [SerializeField] private float turretRotationSpeed;
    public Transform Cam { get; private set; }
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

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //pawn.Health.DeathEvent += OnDeath;
    }

    private void OnDestroy()
    {
        if (Cam != null)
            Cam.transform.parent = null;

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
        //UpdateCursorPosition();
        //pawn.TurretRotate();
        //HullMove();
        //HullRotate();
        //Shoot();
    }
    
    public void UpdateCursorPosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
        {
            currentAimPoint = hit.point;
        }
    }
    
    private void ApplyTurretRotation()
    {
        //rotate the tankTurret block around the Y axis to face the cursor with a speed delay
        //rotate the guntube at a pivot point around the local X  axis to face the cursor with a speed delay
        
        //if the angle between the turret and the cursor is less than 0.1f return
        Vector3 aimDirection = aimCursor.transform.position - tankturret.position;
        float angle = Vector3.Angle(tankturret.forward, aimDirection);
        if (angle < 0.1f)
        {
            //aimCursor is a variable but the type of that variable is a class so class.function applies here
            ShowTargetLock(true);
            return;
        }
        else
        {
            ShowTargetLock(false);
        }
        //gun tube movement isn't working yet, likely due to it being parented to another rotating object
        float yRotation = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
        Quaternion destination = Quaternion.Euler(0, yRotation + 180, 0);
        tankturret.rotation = Quaternion.RotateTowards(tankturret.rotation, destination, turretRotationSpeed * Time.deltaTime);
        
        /* //rotate the turret on the Y axis to face the aimCursor
         Quaternion turretRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
         tankturret.rotation = Quaternion.RotateTowards(tankturret.rotation, turretRotation, turretRotationSpeed * Time.deltaTime);

         //rotate the guntube on the local X axis to face the aimCursor
         Quaternion gunTubeRotation = Quaternion.LookRotation(aimDirection, tankturret.right);
         gunTube.localRotation = Quaternion.RotateTowards(gunTube.localRotation, gunTubeRotation, turretRotationSpeed * Time.deltaTime);
         */
    }

    private void ShowTargetLock(bool b)
    {
        
    }
}
