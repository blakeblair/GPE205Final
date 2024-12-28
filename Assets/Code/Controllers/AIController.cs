using System.Collections.Generic;

public class AIController : Controller
{
    StateMachine stateMachine;

    public AISenses Senses;
    public TankPawn target;
    public AIWaypointing Waypointing;

    private void Awake()
    {
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
        hasTarget.NextState = new ChaseState();

        stateMachine.CurrentState = PatrolState;
        PatrolState.transitions.Add(hasTarget);

        //State ChaseState = new ChaseState();
        //hasTarget.NextState = ChaseState
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
public class ChaseState : State
{
    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        controller.Waypointing.SetDestination(controller.target.transform.position);
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
