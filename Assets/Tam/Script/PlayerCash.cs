using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCash : MonoBehaviour
{
    public int currentCash;
    public UIShop UIShop;
    public TextMeshProUGUI playerCashText;

    // Start is called before the first frame update
    void Start()
    {
        currentCash = 10000;
        playerCashText.text = currentCash.ToString();
        UIShop.SetPlayerCash(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(int amount)
    {
        currentCash += amount;
        playerCashText.text = currentCash.ToString();
    }

    public void CostMoney(int amount)
    {
        currentCash -= amount;
        playerCashText.text = currentCash.ToString();
    }
}
