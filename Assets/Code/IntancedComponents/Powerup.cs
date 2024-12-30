using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        Speed,
        Health,
        Damage
    }

    public PowerupType type;

    public int amount = 2;

    public int respawnTime = 10;

    private void Start()
    {
        respawnTime = Random.Range(0, respawnTime);

        StartCoroutine(GoInactive());
        SetMaterial();
    }

    private void SetMaterial()
    {
        var material = GetComponent<Renderer>().material;

        switch (type)
        {
            case PowerupType.Speed:
                material.SetColor("_Color", Color.cyan);
                break;
            case PowerupType.Health:
                material.SetColor("_Color", Color.green);
                break;
            case PowerupType.Damage:
                material.SetColor("_Color", Color.red);
                break;
        }
    }

    public void SetVisible(bool visible)
    {
        GetComponent<Renderer>().enabled = visible;
        GetComponent<Collider>().enabled = visible;
    }

    IEnumerator GoInactive()
    {
        SetVisible(false);
        yield return new WaitForSeconds(respawnTime);

        type = (PowerupType)UnityEngine.Random.Range(0, 2);

        SetMaterial();

        SetVisible(true);

    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player == null) return;

        var tank = player.GetComponent<TankPawn>();

        if(tank == null) return;

        switch(type)
        {
            case PowerupType.Speed:
                tank.Parameters.hullMoveSpeed += (amount * 0.15f);
                break;
            case PowerupType.Health:
                tank.Health.Heal(20);
                break;
            case PowerupType.Damage:
                tank.Parameters.ShotDamage += amount;
                break;
        }
        StartCoroutine(GoInactive());
    }
}
