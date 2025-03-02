using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSaleSign : MonoBehaviour, IInteractable
{
    private PlayerInteractor currentPlayer;

    public KeyCode keyToInteract => KeyCode.E;

    public void ResetInteractState()
    {
        currentPlayer = null;
    }

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
    }

    void BuyHouse()
    {
        if (HouseManager.instance.BuyHouse(GetComponentInParent<House>()))
        {
            this.gameObject.SetActive(false);
            currentPlayer = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            SystemNotify.instance.SendNotify($"Mua nhà", "Bạn có chắc muốn mua căn nhà này với giá {asd}", BuyHouse, () => { });
        }
    }
}
