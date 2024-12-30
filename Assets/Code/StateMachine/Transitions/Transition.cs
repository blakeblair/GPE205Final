public abstract class Transition
{
    public State NextState;
    public abstract bool Evaluate(AIController controller);
}
