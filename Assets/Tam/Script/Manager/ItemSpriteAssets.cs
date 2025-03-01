using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemSpriteAssets : MonoBehaviour
{
    public static ItemSpriteAssets instance;

    [Header("UI Sprite Assets")]
    public Sprite burgerSprite;
    public Sprite sodaSprite;
    public Sprite bedSprite;
    public Sprite deskSprite;
    public Sprite chairSprite;

    [Header("Prefab Assets")]
    public GameObject bedPrefab;
    public GameObject deskPrefab;
    public GameObject chairPrefab;

    private void Awake()
    {
        instance = this;
    }
}
