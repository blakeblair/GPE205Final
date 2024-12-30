using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int currentHealth = 100;
    public int MaxHealth = 100;

    //PlayerController, TankPawn, and HUD are subscribed to events raised by Health
    public delegate void OnHealthChanged(int health, TankPawn damager);
    public event OnHealthChanged HealthChanged;

    public delegate void OnDeath(TankPawn killer);
    public event OnDeath DeathEvent;

    public TankPawn lastDamager;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            int oldHealth = currentHealth;
            currentHealth = value;

            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            //if health has changed, raise the event
            if (currentHealth != oldHealth)
            {
                HealthChanged?.Invoke(currentHealth, lastDamager);
                Debug.Log(value + "damage taken! " + "Health: " + CurrentHealth);
            }

            //if health has gone from above 0 to 0, raise the death event
            if (oldHealth > 0 && CurrentHealth <= 0)
            {
                DeathEvent?.Invoke(lastDamager);

                lastDamager = null;

                Debug.Log("Dead!");
            }
        }
    }

    public void OnDamageTaken(int damage)
    {
        CurrentHealth -= damage;
    }

    public void SetLastDamager(TankPawn pawn)
    {
        lastDamager = pawn;
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;

    }
}
