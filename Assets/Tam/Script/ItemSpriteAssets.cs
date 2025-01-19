using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemSpriteAssets : MonoBehaviour
{
    public static ItemSpriteAssets instance;
    public Sprite burgerSprite;
    public Sprite sodaSprite;

    private void Awake()
    {
        instance = this;
    }
}
