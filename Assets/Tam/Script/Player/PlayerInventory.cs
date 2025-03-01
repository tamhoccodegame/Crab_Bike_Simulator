using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    private Inventory inventory;
    public ObjectPlacement objectPlacement;

    private PlayerState playerState;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory();
        inventory.onItemUsed += OnItemUsed;
        playerState = GetComponent<PlayerState>();
        objectPlacement.SetInventory(inventory);
    }

    public void AddItem(IShopItem item)
    {
        inventory.AddItem(item);
        UIInventory.instance.RefreshInventoryUI();
    }

    public List<IShopItem> GetItems()
    {
        return inventory.GetItemList();
    }

    public void SetItems(List<IShopItem> items)
    {
        inventory.inventoryItems.Clear();

        if(items != null) 
        foreach(var item in items)
        {
            inventory.AddItem(item);
        }
    }

    public void UseItem(IShopItem item)
    {
        inventory.UseItem(item);
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
        else if(item is Furniture furniture)
        {
            GetComponent<ObjectPlacement>().SetObjectToPlace(furniture.GetPrefab());
        }
        UIInventory.instance.RefreshInventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
