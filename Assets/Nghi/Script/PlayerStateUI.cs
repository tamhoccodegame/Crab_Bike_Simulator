using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStateUI : MonoBehaviour
{
    public PlayerState player;

    public Slider healthSlider;
    public Slider hungerSlider;
    public Slider hygieneSlider;
    public Slider strengthSlider;

    // Start is called before the first frame update

    void Start()
    {
        Debug.Log("Registering OnHealthChange event..."); // Thêm dòng này để kiểm tra số lần đăng ký sự kiện
        player.OnHealthChange -= ChangeHealthUI; // Đảm bảo không bị đăng ký nhiều lần
        //player.OnHealthChange += ChangeHealthUI; // Đăng ký UI update khi máu thay đổi

        // Cập nhật UI ngay từ đầu
        ChangeHealthUI(player.currentHealth);

        player.OnHealthChange += ChangeHealthUI;
        ChangeHealthUI(player.currentHealth);

        player.OnHungerChange += ChangeHungerUI;
        ChangeHungerUI(player.currentHunger);

        player.OnHygieneChange += ChangeHygieneUI;
        ChangeHygieneUI(player.currentHygiene);

        player.OnStrengthChange += ChangeStrengthUI;
        ChangeStrengthUI(player.currentStrength);

        Debug.Log("OnHealthChange subscribed");//để xem có bị gọi nhiều lần không.

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealthUI(float value)
    {
        Debug.Log($"Updating Health UI: {value}"); // Debug kiểm tra UI có cập nhật không
        healthSlider.value = value;

        healthSlider.gameObject.SetActive(false); // Tắt nhanh rồi bật lại để Unity refresh
        healthSlider.gameObject.SetActive(true);
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
