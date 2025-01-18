using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStateUI : MonoBehaviour
{
    public PlayerState player;
    public float abc;

    public Slider healthSlider;
    public Slider hungerSlider;
    public Slider hygieneSlider;
    public Slider strengthSlider;

    // Start is called before the first frame update
    
    void Start()
    {
        player.OnHealthChange += ChangeHealthUI;
        ChangeHealthUI(player.currentHealth);

        player.OnHungerChange += ChangeHungerUI;
        ChangeHungerUI(player.currentHunger);

        player.OnHygieneChange += ChangeHygieneUI;
        ChangeHygieneUI(player.currentHygiene);

        player.OnStrengthChange += ChangeStrengthUI;
        ChangeStrengthUI(player.currentStrength);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealthUI(float value)
    {
        healthSlider.value = value;
    }

    public void ChangeHungerUI(float value)
    {
        hungerSlider.value = value;
    }

    public void ChangeHygieneUI(float value)
    {
        hygieneSlider.value = value;
    }

    public void ChangeStrengthUI(float value)
    {
        strengthSlider.value = value;
    }


}
