using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : IShopItem
{
    public enum FoodType
    {
        Burger,
        Soda,
    }

    public FoodType foodType;


    public Sprite GetSprite()
    {
        switch(foodType)
        {
            case FoodType.Burger:
                return ItemSpriteAssets.instance.burgerSprite;
            case FoodType.Soda:
                return ItemSpriteAssets.instance.sodaSprite;
            default: return null;
        }
    }

    public int GetPrice()
    {
        switch(foodType)
        {
            case FoodType.Burger:
                return 20000;
            case FoodType.Soda:
                return 10000;
            default: 
                return 0;
        }
    }

    public GameObject GetPrefab()
    {
        return null;
    }
}
