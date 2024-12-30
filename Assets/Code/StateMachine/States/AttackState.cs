using UnityEngine;

public class AttackState : State
{
    public override void OnStateEnter(AIController controller)
    {
        base.OnStateEnter(controller);

        controller.lostSightTimer = 0f;

        if (controller.Senses.SensedEnemies.Count > 0)
        {
            controller.target = controller.Senses.SensedEnemies[0];
            controller.lastSeenPostion = controller.target.transform.position;
        }

        if(controller.Senses.HeardEnemies.Count > 0)
        {
            controller.target = controller.Senses.HeardEnemies[0];
            controller.lastSeenPostion = controller.target.transform.position;
        }
    }

    public override void OnStateUpdate(AIController controller)
    {
        base.OnStateUpdate(controller);

        controller.Waypointing.SetDestination(controller.target.transform.position);

        var collider = controller.target.GetComponent<Collider>();
        if (controller.Senses.HasClearShot(collider))
            controller.pawn.Shoot();

        if(!controller.Senses.HasLos(collider))
        {
            controller.LostSightUpdate();
        }
        else
        {
            controller.GainedSightUpdate();
        }
    }

    public override void OnStateExit(AIController controller)
    {
        base.OnStateExit(controller);

        controller.lostSightTimer = 0f;

        controller.Waypointing.SetDestination(controller.lastSeenPostion);

        controller.target = null;
    }

}
