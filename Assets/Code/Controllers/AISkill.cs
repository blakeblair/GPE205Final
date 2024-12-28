﻿using UnityEngine;

[CreateAssetMenu(fileName = "AISKill", menuName = "AI/Skill")]
public class AISkill : ScriptableObject
{
    public float detectionRadius = 5;
    public float detectionTime = 1;
    public float detectionFov = 90;

    public float health = 100;
    public float FleeThreshold = 100;
}
