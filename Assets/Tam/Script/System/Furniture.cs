using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Furniture : MonoBehaviour, IShopItem
{
    public enum FurnitureType
    {
        Bed,
        Wardrobe,
    }
    public FurnitureType type;

    public int GetPrice()
    {
        switch(type)
        {
            case FurnitureType.Bed:
                return 50000;
            case FurnitureType.Wardrobe:
                return 80000;
            default: return 0;
        }
    }

    public Sprite GetSprite()
    {
        return ItemSpriteAssets.instance.sodaSprite;
    }

    public GameObject GetPrefab()
    {
        return ItemSpriteAssets.instance.bedPrefab;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
