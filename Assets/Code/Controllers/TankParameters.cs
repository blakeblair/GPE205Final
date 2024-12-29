using UnityEngine;

[CreateAssetMenu(fileName = "TankParameters", menuName = "Tank/Parameters")]
public class TankParameters : ScriptableObject
{
    public int MaxHealth = 100;
    public int ShotDamage = 25;
    public float ShootFrequency = 0.75f;
    public float ShotForce = 10f;

    public float hullMoveSpeed = 5;
    public float hullRotationSpeed = 25;
    public float cameraSpeed = 10;
    public float turretRotationSpeed = 30;

}
