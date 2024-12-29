using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    [SerializeField] public Bullet bulletPrefab;
    [SerializeField] public Transform firePoint;

    public abstract void Shoot();

}
