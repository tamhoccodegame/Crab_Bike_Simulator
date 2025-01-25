using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopItem
{
    Sprite GetSprite();
    GameObject GetPrefab();
    int GetPrice();
}
