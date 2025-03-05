using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager instance;
    public House currentOwnHouse;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onSceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(SaveData data)
    {
        House[] allHouses = FindObjectsOfType<House>();
        foreach (House house in allHouses)
        {
            if (house.id == data.ownHouseId)
            {
                currentOwnHouse = house;
                currentOwnHouse.isOwned = true;
                currentOwnHouse.houseDoor.enabled = true;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool BuyHouse(House _house)
    {
        if(currentOwnHouse != null)
        {
            Debug.Log("Bạn đang có nhà rồi! Không thể mua");
            return false;
        }

        if (PlayerCash.instance.CostMoney(_house.price))
        {
            currentOwnHouse = _house;
            currentOwnHouse.isOwned = true;
            currentOwnHouse.houseDoor.enabled = true;
            return true;
        }

        return false;
    }

    public void SellHouse()
    {
        if(currentOwnHouse == null)
        {
            Debug.Log("Có nhà đâu mà bán?");
            return;
        }

        PlayerCash.instance.AddMoney(Mathf.FloorToInt(currentOwnHouse.price / 2f));
        currentOwnHouse.isOwned = false;
        currentOwnHouse.houseDoor.enabled = false;
        currentOwnHouse = null;
    }
}
