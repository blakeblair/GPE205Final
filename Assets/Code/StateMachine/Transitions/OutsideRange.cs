using UnityEngine;

public class OutsideRange : Transition
{
    public override bool Evaluate(AIController controller)
    {
        if (controller.target == null) return true;

        var dist = Vector3.Distance(controller.target.transform.position, controller.transform.position);
        return dist > Mathf.Max(controller.Senses.Skill.sightRadius, controller.Senses.Skill.hearingRadius);
    }
}
