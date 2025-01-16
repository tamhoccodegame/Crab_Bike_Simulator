using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    private int playerCurrency;

    void Start()
    {
        LoadCurrency();
    }

    // Save currency to PlayerPrefs
    public void SaveCurrency()
    {
        PlayerPrefs.SetInt("PlayerCurrency", playerCurrency);
        PlayerPrefs.Save();  // Save changes immediately
    }

    // Load currency from PlayerPrefs
    public void LoadCurrency()
    {
        playerCurrency = PlayerPrefs.GetInt("PlayerCurrency", 0);  // Default value is 0
    }

    // Add currency
    public void AddCurrency(int amount)
    {
        playerCurrency += amount;
        SaveCurrency();  // Save after adding
    }

    // Subtract currency
    public void SubtractCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            playerCurrency -= amount;
            SaveCurrency();  // Save after subtracting
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }

    // Display the currency in the console
    public void DisplayCurrency()
    {
        Debug.Log("Current Currency: " + playerCurrency);
    }
}


