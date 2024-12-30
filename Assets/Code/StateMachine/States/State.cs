using System.Collections.Generic;

public abstract class State
{
    public List<Transition> transitions = new List<Transition>();

    //State availability is assessed on update, checking each transition for availability
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
