using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class InventoryReplace
{
    public string typeName;
    public string itemName;
}

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public int playerCash;
    public List<InventoryReplace> inventoryItems;
    public List<string> ownVehiclesName;
    public int ownHouseId;
}

public class SaveLoadManager
{
    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.playerPosition = TPlayerController.instance.transform.position;
        data.playerCash = PlayerCash.instance.currentCash;
        data.inventoryItems = new List<InventoryReplace>();

        List<IShopItem> items = PlayerInventory.instance.GetItems();

        foreach (IShopItem item in items)
        {
            if(item is Food food)
            {
                data.inventoryItems.Add(new InventoryReplace { typeName = "Food", itemName = food.foodType.ToString() });
            }
            else if(item is Furniture furniture)
            {
                data.inventoryItems.Add(new InventoryReplace { typeName = "Furniture", itemName = furniture.type.ToString() });
            }
        }

        data.ownVehiclesName = new List<string>();

        foreach(GameObject vehicle in VehicleManager.instance.ownVehicles)
        {
            data.ownVehiclesName.Add(vehicle.name);
        }

        if(HouseManager.instance.currentOwnHouse != null)
        data.ownHouseId = HouseManager.instance.currentOwnHouse.id;

        string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else return null;
    }
}
