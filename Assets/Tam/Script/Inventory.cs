using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<IShopItem> inventoryItems = new List<IShopItem>();
    public Action<IShopItem> onItemUsed;
    public Action onInventoryChanged;

    public Inventory()
    {
        inventoryItems.Add(new Food { foodType = Food.FoodType.Burger });
        inventoryItems.Add(new Food { foodType = Food.FoodType.Soda });
    }

    public List<IShopItem> GetItemList()
    {
        return inventoryItems;
    }

    public void AddItem(IShopItem item)
    {
        inventoryItems.Add(item);
        onInventoryChanged?.Invoke();
    }

    public void UseItem(IShopItem item)
    {
        inventoryItems.Remove(item);
        onItemUsed?.Invoke(item);
        onInventoryChanged?.Invoke();
    }
}
