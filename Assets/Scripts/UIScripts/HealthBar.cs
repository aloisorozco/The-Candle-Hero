using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    public void SetHealth(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;

    }
}
