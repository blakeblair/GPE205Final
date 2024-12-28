using System.Collections;
using System.Collections.Generic;

public class AIController : Controller
{
    StateMachine stateMachine;

    public AISenses Senses;
    public TankPawn target;

    private void Awake()
    {
        InitStateMachine();
    }

    private void Start()
    {
        Senses = GetComponent<AISenses>();
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();

        State PatrolState = new PatrolState();
        Transition hasTarget = new HasTargetEnoughHealth();
        stateMachine.CurrentState = PatrolState;

        //State ChaseState = new ChaseState();
        //hasTarget.NextState = ChaseState


        PatrolState.transitions.Add(hasTarget);

    }

    public void Transition(State state)
    {
        stateMachine.CurrentState = state;
        stateMachine.CurrentState.OnStateEnter(this);
    }

    private void Update()
    {
        if(stateMachine.CurrentState.EvaluateTransitions(this))
        {

        }
        

        stateMachine.Update(this);
    }


}

public class PatrolState : State
{

    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        if(!controller.Senses.IsNavigatingToWaypoint)
        {
            var randomWaypoint = GameManager.Instance.Waypoints[UnityEngine.Random.Range(0, GameManager.Instance.Waypoints.Length)].position;
            controller.Senses.GotoWaypoint(randomWaypoint);
        }

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
                controller.Transition(this);
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
