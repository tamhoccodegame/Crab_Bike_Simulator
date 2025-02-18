using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    public GameObject healthBarPrefab;
    public Canvas canvas; // Canvas chính của UI (World Space hoặc Screen Space)

    //public Transform canvasTransform; // Gán canvas chứa thanh máu

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public GameObject GetHealthBar()
    {
        if (healthBarPrefab == null)
        {
            Debug.LogError("HealthBar Prefab chưa được gán trong UI_Manager!");
        }
        return healthBarPrefab;
    }

    //public NPC_Health_UI CreateHealthBar(Transform npc)
    //{
    //    GameObject heathBarInstance = Instantiate(healthBarPrefab, canvasTransform);
    //    NPC_Health_UI healthBar = heathBarInstance.GetComponent<NPC_Health_UI>();
    //    healthBar.target = npc;
    //    return healthBar;
    //}
}
