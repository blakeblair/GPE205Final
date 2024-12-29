using UnityEngine;

[CreateAssetMenu(fileName = "AISKill", menuName = "Tank/AI/Skill")]
public class AISkill : ScriptableObject
{
    public TankParameters CustomParameters;

    public float detectionRadius = 5f;
    public float lostSightTime = 1f;
    public float detectionFov = 90f;

    public float FleeThreshold = 50f;
}
