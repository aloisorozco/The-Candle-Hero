using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            transform.parent.gameObject.SetActive(true);
        }
    }

    public void SetHealth(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;

    }
}
