using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIController : Controller
{
    StateMachine stateMachine;

    public AISenses Senses;
    public TankPawn target;
    public AIWaypointing Waypointing;
    public TankAudio TankAudio;

    public float lostSightTime;
    public float lostSightTimer;

    public Vector3 lastSeenPostion;

    bool hasSight;

    protected override void Awake()
    {
        base.Awake();
        TankAudio = GetComponent<TankAudio>();
        pawn.Health.DeathEvent += OnDeath;
        InitStateMachine();
    }

    private void OnDeath(TankPawn killer)
    {
        GameManager.AddScore(Senses.Skill.value);
        Waypointing.enabled = false;
        pawn.Movement.enabled = false;
        Senses.enabled = false;
        Destroy(gameObject, 2.0f);
    }

    private void Start()
    {
        Waypointing = GetComponent<AIWaypointing>();
        Senses = GetComponent<AISenses>();

    }
    //Each AITank has an AIController that has a StateMachine,
    //this avoids the complexity of one state machine handling all the AI tanks like in the previous project
    private void InitStateMachine()
    {
        stateMachine = new StateMachine();

        //Create States
        State PatrolState = new PatrolState();
        State FleeState = new FleeState();
        State FireState = new AttackState();

        //Create Transitions
        Transition hasTarget = new HasTargetEnoughHealth();
        hasTarget.NextState = FireState;

        Transition outsideRange = new OutsideRange();
        outsideRange.NextState = PatrolState;

        Transition lostSight = new LostSight();
        lostSight.NextState = PatrolState;

        Transition heardTarget = new HeardTarget();
        heardTarget.NextState = FireState;

        Transition hasTargetLow = new HasTargetLowHealth();
        hasTargetLow.NextState = new FleeState();

        Transition noTarget = new NoTarget();
        noTarget.NextState = PatrolState;

        //Assign Transitions
        FleeState.transitions.Add(outsideRange);
        FleeState.transitions.Add(noTarget);

        FireState.transitions.Add(outsideRange);
        FireState.transitions.Add(lostSight);
        FireState.transitions.Add(hasTargetLow);

        PatrolState.transitions.Add(hasTarget);
        PatrolState.transitions.Add(heardTarget);

        stateMachine.CurrentState = PatrolState;

    }

    public override void ProcessInputs()
    {
        base.ProcessInputs();
    }

    public void Transition(State state)
    {
        if(state == null)
        {
            Debug.Log("uh oh no state available");
            return;
        }
        stateMachine.CurrentState = state;
        stateMachine.CurrentState.OnStateEnter(this);
    }

    private void Update()
    {
        if (pawn.Health.CurrentHealth <= 0) return;

        if (stateMachine.CurrentState == null)
        {
            Debug.LogError("Null state", gameObject);
            return;
        }
        stateMachine.CurrentState.EvaluateTransitions(this);

        stateMachine.Update(this);


        //DebugPlus.LogOnScreen("Current State: " + stateMachine.CurrentState.GetType());
        //DebugPlus.LogOnScreen("lostSightTimer: " + lostSightTimer);
    }

    public void LostSightUpdate()
    {
        lostSightTimer += Time.deltaTime;
        //lostSightTime is a personality variable for AI tanks, some tanks will give up quicker than others
        if(lostSightTimer > Senses.Skill.lostSightTime)
        {
            if(hasSight == true)
            {
                OnLostSight();
                hasSight = false;
            }
        }
    }

    private void OnLostSight()
    {
        //saves the position at which the AI last saw the player, allowing it to go to that position before resuming patrol state
        lastSeenPostion = target.transform.position;
    }

    internal void GainedSightUpdate()
    {
        if (hasSight == false)
        {
            OnGainedSight();
            hasSight = true;
        }
    }

    private void OnGainedSight()
    {
        lostSightTimer = 0f;
    }
}
