public class PatrolState : State
{

    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        if(!controller.Waypointing.IsNavigatingToWaypoint)
        {
            var randomWaypoint = GameManager.Instance.RandomWaypoint.position;
            controller.Waypointing.SetDestination(randomWaypoint);
        }

    }
}
