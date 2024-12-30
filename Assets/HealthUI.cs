using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public TankPawn pawn;
    public Slider slider;

    private void Start()
    {
        if(pawn.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
            return;
        }
        pawn.Health.HealthChanged += Health_HealthChanged;
        slider.value = 1f;

    }

    private void Health_HealthChanged(int health, TankPawn damager)
    {
        slider.value = ((float)health / (float)pawn.Health.MaxHealth);

        if(slider.value <= 0)
        {
            slider.gameObject.SetActive(false);
        }
    }
}
