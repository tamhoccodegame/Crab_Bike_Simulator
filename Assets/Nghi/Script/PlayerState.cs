using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 100f, maxHunger = 100f, maxHygiene = 100f, maxStrength = 100f;
    public float currentHealth, currentHunger, currentHygiene, currentStrength;

    public event Action<float> OnHealthChange;
    public event Action<float> OnHungerChange;
    public event Action<float> OnHygieneChange;
    public event Action<float> OnStrengthChange;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHygiene = maxHygiene;
        currentStrength = maxStrength;

        InvokeRepeating(nameof(DecreaseHunger), 1f, 1f);
        InvokeRepeating(nameof(DecreaseHygiene), 2f, 2f);

        OnHealthChange?.Invoke(currentHealth);
        OnHungerChange?.Invoke(currentHunger);
        OnStrengthChange?.Invoke(currentStrength);
        OnHygieneChange?.Invoke(currentHygiene);

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
        if (currentHealth <= 0) return; // Không giảm nữa nếu đã chết.

        currentHealth -= amount;

        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);

        //Debug.Log($"Health changed: {currentHealth}"); // Debug kiểm tra giá trị máu

        if (OnHealthChange != null)
        {
            //Debug.Log("Invoking OnHealthChange event..."); // Kiểm tra sự kiện có được gọi
            OnHealthChange.Invoke(currentHealth);
        }
        else
        {
            //Debug.LogWarning("OnHealthChange is null, UI won't update!");
        }


        if (currentHealth <= 0)
        {
            //Debug.Log("Player is Dead!");
            CancelInvoke();
        }
    }

    public void DecreaseStrength(float amount)
    {
        currentStrength -= amount;  

        currentStrength = Mathf.Clamp(currentStrength, 0, 100);

        OnStrengthChange?.Invoke(currentStrength);
    }

    public void AddHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, 100);
        OnHungerChange?.Invoke(currentHunger);
    }

    public void AddHygiene(float amount)
    {
        
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //Debug.Log($"Healing {amount}. Current Health: {currentHealth}");

        OnHealthChange?.Invoke(currentHealth); // Gọi sự kiện để cập nhật UI
    }

    public void AddStrength(float amount)
    {
        
    }
}
