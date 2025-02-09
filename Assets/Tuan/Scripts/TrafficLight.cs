using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightState { Red, Green, Yellow }
    public LightState currentLightState = LightState.Green;
    public float greenTime = 10f;
    public float redTime = 10f;
    public float yellowTime = 3f;

    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        switch (currentLightState)
        {
            case LightState.Green:
                SetActiveChildByName("Green", true);
                if (timer >= greenTime)
                {
                    currentLightState = LightState.Yellow;
                    SetActiveChildByName("Yellow", true);
                    SetActiveChildByName("Green", false);
                    timer = 0f;
                }
                break;
            case LightState.Yellow:
                if (timer >= yellowTime)
                {
                    currentLightState = LightState.Red;
                    SetActiveChildByName("Red", true);
                    SetActiveChildByName("Yellow", false);
                    timer = 0f;
                }
                break;
            case LightState.Red:
                if (timer >= redTime)
                {
                    currentLightState = LightState.Green;
                    SetActiveChildByName("Green", true);
                    SetActiveChildByName("Red", false);
                    timer = 0f;
                }
                break;
        }
    }
    void SetActiveChildByName(string childName, bool isActive)
    {
        
        foreach (Transform child in transform)
        {
            if (child.name == childName)
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }
  

}

