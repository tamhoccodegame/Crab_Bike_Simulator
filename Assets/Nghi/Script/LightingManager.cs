﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    private int day = 1;

    private float secondsPerGameHour = 10f; // 10 giây thực tế = 1 giờ trong game

    public Text timeText;
    public Text dayText;

    private void Start()
    {
        timeText.text = "Time: " + TimeOfDay;
        dayText.text = "Day: " + day;
    }

    private void Update()
    {
        UpdateTimeOnUI();   
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime/secondsPerGameHour;
            //TimeOfDay %= 24; //Modulus to ensure always between 0-24
            
            if (TimeOfDay >=24f)
            {
                // Đưa thời gian về khoảng 0-24 (reset khi qua ngày mới)
                TimeOfDay -= 24f;
                day += 1;
                UpdateTimeOnUI();
            }
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    public void UpdateTimeOnUI()
    {
        int hours = Mathf.FloorToInt(TimeOfDay); // Lấy phần giờ
        int minutes = Mathf.FloorToInt((TimeOfDay - hours) * 60); // Lấy phần phút

        timeText.text = $"Time: {hours:D2}:{minutes:D2}";
        dayText.text = "Day: " + day;
    }
}
