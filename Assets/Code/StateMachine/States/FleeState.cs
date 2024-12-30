public class FleeState : State
{
    public override void OnStateEnter(AIController controller)
    {
        base.OnStateEnter(controller);
    }

    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        if (controller.target == null) return;

        var away = (controller.target.transform.position - controller.transform.position).normalized;
        controller.Waypointing.SetDestination(away * 2.5f);
    }

    public override void OnStateExit(AIController controller)
    {
        base.OnStateExit(controller);

        controller.target = null;
    }

}
