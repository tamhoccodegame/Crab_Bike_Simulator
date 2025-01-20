using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShop : MonoBehaviour, IShop
{
    public List<IShopItem> foodList = new List<IShopItem>();

    public List<IShopItem> GetShopItems()
    {
        return foodList;
    }

    // Start is called before the first frame update
    void Awake()
    {
        foodList.Add(new Food { foodType = Food.FoodType.Burger } as IShopItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
