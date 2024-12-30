public class NoTarget : Transition
{
    public override bool Evaluate(AIController controller)
    {
        return controller.Senses.SensedEnemies.Count < 1 && controller.Senses.HeardEnemies.Count < 1;
    }
}
