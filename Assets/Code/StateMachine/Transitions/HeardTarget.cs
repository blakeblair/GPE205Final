public class HeardTarget : Transition
{
    public override bool Evaluate(AIController controller)
    {
        return controller.Senses.HeardEnemies.Count > 0;
    }
}
