using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fill;

    public static HealthBarUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateFill(int currentHealth, int maxHealth)
    {
        fill.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}
