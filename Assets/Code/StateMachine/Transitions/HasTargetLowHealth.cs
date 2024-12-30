public class HasTargetLowHealth : Transition
{
    public override bool Evaluate(AIController controller)
    {
        var hasEnemy = controller.Senses.SensedEnemies.Count > 0 || controller.Senses.HeardEnemies.Count > 0;
        return hasEnemy && controller.pawn.Health.CurrentHealth <= controller.Senses.Skill.FleeThreshold;
    }
}
