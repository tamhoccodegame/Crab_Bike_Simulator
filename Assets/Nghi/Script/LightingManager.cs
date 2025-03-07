﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    public static LightingManager instance;
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private Material sunriseSkyBox;
    [SerializeField] private Material morningSkyBox;
    [SerializeField] private Material afternoonSkyBox;
    [SerializeField] private Material sunsetSkyBox;
    [SerializeField] private Material nightSkyBox;

    //[SerializeField] private Mater
    //Variables
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    private int day = 1;

    public AudioSource openingDayAudio;
    public TextMeshProUGUI TimeOfDayBig;

    private float secondsPerGameHour = 60f; // 60 giây thực tế = 1 giờ trong game

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;

    private GameObject[] lights;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        timeText.text = "Time: " + TimeOfDay;
        dayText.text = "Day: " + day;
        lights = GameObject.FindGameObjectsWithTag("Light");
    }

    private void Update()
    {
        UpdateTimeOnUI();
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime / secondsPerGameHour;
            //TimeOfDay %= 24; //Modulus to ensure always between 0-24

            UpdateLight();
            UpdateSkyBox();
            TriggerSpecialEvent();

            if (TimeOfDay >= 24f)
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

    void TriggerSpecialEvent()
    {
        int hours = Mathf.FloorToInt(TimeOfDay);
        if (Mathf.FloorToInt((TimeOfDay - hours) * 60) > 0) return;
        if (5 == hours)
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.Texting);
            if(!openingDayAudio.isPlaying)
            openingDayAudio.Play();
            TimeOfDayBig.gameObject.SetActive(false);
            return;
        }
        if(24 == hours)
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.Sleeping);
            SystemNotify.instance.SendBigNoti("Bạn đang ngủ.....", Color.white);
            TimeOfDayBig.gameObject.SetActive(true);
            return;
        }
    }

    private void UpdateLight()
    {
        if (TimeOfDay >= 7.5f && TimeOfDay < 18f)
        {
            // Tắt đèn vào ban ngày
            foreach (var light in lights)
            {
                if (light.activeSelf) // Chỉ tắt nếu đèn đang bật
                {
                    light.SetActive(false);
                }
            }
        }
        else
        {
            // Bật đèn vào ban đêm
            foreach (var light in lights)
            {
                if (!light.activeSelf) // Chỉ bật nếu đèn đang tắt
                {
                    light.SetActive(true);
                }
            }
        }
    }

    private void UpdateSkyBox()
    {
        //Bình minh
        if(TimeOfDay >= 5.5f && TimeOfDay < 7f)
        {
            RenderSettings.skybox = sunriseSkyBox;
        }
        //Sáng
        else if(TimeOfDay >= 7f && TimeOfDay < 11f)
        {
            RenderSettings.skybox = morningSkyBox;
        }
        //Trưa
        else if(TimeOfDay >= 11f && TimeOfDay < 17.5f)
        {
            RenderSettings.skybox = afternoonSkyBox;
        }
        //Hoàng hôn
        else if(TimeOfDay >= 17.5f && TimeOfDay < 19f)
        {
            RenderSettings.skybox = sunsetSkyBox;
        }
        //Tối
        else
        {
            RenderSettings.skybox = nightSkyBox; 
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
        TimeOfDayBig.text = $"{hours:D2}:{minutes:D2}";
        dayText.text = "Day: " + day;
    }

    public void SetDaySpeed(float speed)
    {
        secondsPerGameHour = speed;
    }
}
