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
    public Sprite wardrobeSprite;

    [Header("Prefab Assets")]
    public GameObject burgerPrefab;
    public GameObject sodaPrefab;
    public GameObject bedPrefab;
    public GameObject wardrobePrefab;

    private void Awake()
    {
        instance = this;
    }
}
