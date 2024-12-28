using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int currentHealth = 100;
    public int MaxHealth { get; private set; } = 100;

    public delegate void OnHealthChanged(int health);
    public event OnHealthChanged HealthChanged;

    public delegate void OnDeath();
    public event OnDeath DeathEvent;

    public TankPawn pawn;
    public TankPawn lastDamager;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            int oldHealth = currentHealth;
            currentHealth = value;

            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

            if (currentHealth != oldHealth)
            {
                HealthChanged?.Invoke(currentHealth);
                Debug.Log(value + "damage taken! " + "Health: " + CurrentHealth);
            }

            if (oldHealth > 0 && CurrentHealth <= 0)
            {
                DeathEvent?.Invoke();

                GiveKillerScore();
                lastDamager = null;

                Debug.Log("Dead!");
            }
        }
    }

    private void GiveKillerScore()
    {
        if (lastDamager != null)
        {
            var controller = lastDamager.GetComponent<PlayerController>();
            if (controller)
            {
                controller.AddScore(10);
            }
        }
    }

    private void Awake()
    {
        pawn = GetComponent<TankPawn>();
        CurrentHealth = MaxHealth;
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
