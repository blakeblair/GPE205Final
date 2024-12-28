using System.Collections;
using System.Collections.Generic;

public class AIController : Controller
{
    private void Awake()
    {
        
    }

    public override void ProcessInputs()
    {
        
    }

}

public class StateMachine
{
    public State CurrentState;
}

public abstract class State
{
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
