public class StateMachine
{
    public State CurrentState;

    public void Update(AIController controller)
    {
        CurrentState.OnStateUpdate(controller);
    }
}
