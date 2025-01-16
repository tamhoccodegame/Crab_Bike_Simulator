using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public CurrencyManager currencyManager;

    public void BuyItem()
    {
        int itemCost = 50; // Example item cost
        currencyManager.SubtractCurrency(itemCost);
    }

    public void EarnMoney()
    {
        int reward = 100; // Example reward
        currencyManager.AddCurrency(reward);
    }
}

