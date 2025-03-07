using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCash : MonoBehaviour
{
    public static PlayerCash instance;
    public int currentCash;
    public TextMeshProUGUI playerCashText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCash = 500000;
        playerCashText.text = currentCash.ToString("N0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(int amount)
    {
        currentCash += amount;
        playerCashText.text = currentCash.ToString("N0");
    }

    public bool CostMoney(int amount)
    {
        if (currentCash < amount) return false;
        
        currentCash -= amount;
        playerCashText.text = currentCash.ToString("N0");
        return true;
    }
}
