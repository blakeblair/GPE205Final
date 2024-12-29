using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIController : Controller
{
    StateMachine stateMachine;

    public AISenses Senses;
    public TankPawn target;
    public AIWaypointing Waypointing;

    public float lostSightTime;
    public float lostSightTimer;

    public Vector3 lastSeenPostion;

    bool hasSight;

    protected override void Awake()
    {
        base.Awake();
        InitStateMachine();
    }

    private void Start()
    {
        Waypointing = GetComponent<AIWaypointing>();
        Senses = GetComponent<AISenses>();
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();

        State PatrolState = new PatrolState();

        Transition hasTarget = new HasTargetEnoughHealth();
        hasTarget.NextState = new AttackState();


        stateMachine.CurrentState = PatrolState;
        PatrolState.transitions.Add(hasTarget);

        State FireState = new AttackState();
        hasTarget.NextState = FireState;

        Transition outsideRange = new OutsideRange();
        outsideRange.NextState = PatrolState;

        Transition lostSight = new LostSight();
        lostSight.NextState = PatrolState;


        FireState.transitions.Add(outsideRange);
        FireState.transitions.Add(lostSight); 
    }

    public override void ProcessInputs()
    {
        base.ProcessInputs();
    }

    public void Transition(State state)
    {
        stateMachine.CurrentState = state;
        stateMachine.CurrentState.OnStateEnter(this);
    }

    private void Update()
    {
        stateMachine.CurrentState.EvaluateTransitions(this);

        stateMachine.Update(this);


        DebugPlus.LogOnScreen("Current State: " + stateMachine.CurrentState.GetType());
        DebugPlus.LogOnScreen("lostSightTimer: " + lostSightTimer);
    }

    public void LostSightUpdate()
    {
        lostSightTimer += Time.deltaTime;

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

public class PatrolState : State
{

    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        if(!controller.Waypointing.IsNavigatingToWaypoint)
        {
            var randomWaypoint = GameManager.Instance.RandomWaypoint.position;
            controller.Waypointing.SetDestination(randomWaypoint);
        }

    }
}

public class AttackState : State
{
    public override void OnStateEnter(AIController controller)
    {
        base.OnStateEnter(controller);

        controller.lostSightTimer = 0f;

        if (controller.Senses.SensedEnemies.Count < 1)
            return;

        controller.target = controller.Senses.SensedEnemies[0];
        controller.lastSeenPostion = controller.target.transform.position;
    }

    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        

        controller.Waypointing.SetDestination(controller.target.transform.position);

        var collider = controller.target.GetComponent<Collider>();
        if (controller.Senses.HasClearShot(collider))
            controller.pawn.Shoot();

        if(!controller.Senses.HasLos(collider))
        {
            controller.LostSightUpdate();
        }
        else
        {
            controller.GainedSightUpdate();
        }
    }

    public override void OnStateExit(AIController controller)
    {
        base.OnStateExit(controller);

        controller.lostSightTimer = 0f;

        controller.Waypointing.SetDestination(controller.lastSeenPostion);

        controller.target = null;
    }

}

public class StateMachine
{
    public State CurrentState;

    public void Update(AIController controller)
    {
        CurrentState.OnStateUpdate(controller);
    }
}

public class HasTargetEnoughHealth : Transition
{
    public override bool Evaluate(AIController controller)
    {
        return controller.Senses.SensedEnemies.Count > 0 && controller.pawn.Health.CurrentHealth > controller.Senses.Skill.FleeThreshold;
    }
}

public class HasTargetLowHealth : Transition
{
    public override bool Evaluate(AIController controller)
    {
        return controller.Senses.SensedEnemies.Count > 0 && controller.pawn.Health.CurrentHealth <= controller.Senses.Skill.FleeThreshold;
    }
}

public class LostSight : Transition
{
    public override bool Evaluate(AIController controller)
    {
        return controller.lostSightTimer > controller.Senses.Skill.lostSightTime;
    }
}

public class OutsideRange : Transition
{
    public override bool Evaluate(AIController controller)
    {
        if (controller.target == null) return true;

        var dist = Vector3.Distance(controller.target.transform.position, controller.transform.position);
        DebugPlus.LogOnScreen(dist);
        return dist > controller.Senses.Skill.detectionRadius;
    }
}

public abstract class Transition
{
    public State NextState;
    public abstract bool Evaluate(AIController controller);
}

public abstract class State
{
    public List<Transition> transitions = new List<Transition>();

    public bool EvaluateTransitions(AIController controller)
    {
        for(int i = 0; i < transitions.Count; i++)
        {
            var result = transitions[i].Evaluate(controller);
            if(result == true)
            {
                OnStateExit(controller);
                controller.Transition(transitions[i].NextState);
                return true;
            }
        }

        return true;
    }

    public virtual void OnStateEnter(AIController controller)
    {

    }

    public virtual void OnStateUpdate(AIController controller)
    {

    }

    public virtual void OnStateExit(AIController controller)
    {

    }
}
