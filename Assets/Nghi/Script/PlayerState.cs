using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float maxHealth = 100f, maxHunger = 100f, maxHygiene = 100f, maxStrength = 100f;
    public float currentHealth, currentHunger, currentHygiene, currentStrength;

    public Action<float> OnHealthChange;
    public Action<float> OnHungerChange;
    public Action<float> OnHygieneChange;
    public Action<float> OnStrengthChange;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHygiene = maxHygiene;
        currentStrength = maxStrength;

        InvokeRepeating(nameof(DecreaseHunger), 1f, 1f);
        InvokeRepeating(nameof(DecreaseHygiene), 2f, 2f);

        OnHealthChange(currentHealth);
        OnStrengthChange(currentStrength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseHunger()
    {
        currentHunger -= 1f;
        currentHunger = Math.Clamp(currentHunger, 0f, 100f);//Dam bao gia tri trong khoang 0-100

        OnHungerChange?.Invoke(currentHunger);

        if (currentHunger == 0)
        {
            DecreaseHealth(2);
        }

    }

    public void DecreaseHygiene()
    {
        currentHygiene -= 1;

        currentHygiene = Math.Clamp(currentHygiene, 0, 100);

        OnHygieneChange?.Invoke(currentHygiene);

        if (currentHygiene == 0)
        {
            DecreaseHealth(1);
        }
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        currentHealth = Math.Clamp(currentHealth, 0, 100);

        OnHealthChange?.Invoke(currentHealth);

        if (currentHealth < 0)
        {
            Debug.Log("Player died!");
            CancelInvoke();
        }
    }

    public void DecreaseStrength(float amount)
    {
        currentStrength -= amount;  

        currentStrength = Mathf.Clamp(currentStrength, 0, 100);

        OnStrengthChange?.Invoke(currentStrength);

        if (currentStrength == 0)
        {
            DecreaseHealth(5);
        }
    }
}
