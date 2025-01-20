using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;
    public UIInventory UIInventory;
    public UIShop UIShop;

    private PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory();
        inventory.onItemUsed += OnItemUsed;
        playerState = GetComponent<PlayerState>();
        UIInventory.SetInventory(inventory);
        UIShop.SetPlayerInventory(inventory);
    }

    void OnItemUsed(IShopItem item)
    {
        if(item is Food food)
        {
            switch (food.foodType)
            {
                //Call To Player Stat Reference
                case Food.FoodType.Burger:
                    playerState.AddHunger(5);
                    break;
                case Food.FoodType.Soda:
                    playerState.AddStrength(5);
                    break;
            }
        }
        //Weapon and Furniture
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
