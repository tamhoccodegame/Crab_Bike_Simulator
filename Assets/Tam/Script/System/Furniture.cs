using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Furniture : MonoBehaviour, IShopItem
{
    public enum FurnitureType
    {
        Bed,
        Desk,
        Chair,
    }
    public FurnitureType type;

    public int GetPrice()
    {
        switch(type)
        {
            case FurnitureType.Bed:
                return 50000;
            case FurnitureType.Desk:
                return 80000;
            case FurnitureType.Chair:
            default: return 0;
        }
    }

    public Sprite GetSprite()
    {
        return ItemSpriteAssets.instance.sodaSprite;
    }

    public GameObject GetPrefab()
    {
        switch (type)
        {
            case FurnitureType.Bed:
                return ItemSpriteAssets.instance.bedPrefab;
            case FurnitureType.Desk:
                return ItemSpriteAssets.instance.deskPrefab;
            case FurnitureType.Chair:
                return ItemSpriteAssets.instance.chairPrefab;
            default: return null;
        }
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
