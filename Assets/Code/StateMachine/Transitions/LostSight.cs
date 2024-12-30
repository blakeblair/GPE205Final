public class LostSight : Transition
{
    public override bool Evaluate(AIController controller)
    {
        return controller.lostSightTimer > controller.Senses.Skill.lostSightTime;
    }
}
