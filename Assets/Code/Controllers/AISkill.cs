using UnityEngine;

[CreateAssetMenu(fileName = "AISKill", menuName = "Tank/AI/Skill")]
public class AISkill : ScriptableObject
{
    public Color TankColor;

    //prefab personalities, if set overrides the tank stats
    public TankParameters CustomParameters;
    
    public float sightRadius = 5f;
    public float hearingRadius = 15f;
    public float lostSightTime = 1f;
    public float detectionFov = 90f;

    public float FleeThreshold = 50f;
    public int value = 1;
}
