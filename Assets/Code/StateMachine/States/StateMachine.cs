public class StateMachine
{
    public State CurrentState;

    //calls the Update method of the current state
    public void Update(AIController controller)
    {
        CurrentState.OnStateUpdate(controller);
    }
}
