using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSaleSign : MonoBehaviour, IInteractable
{
    public void OnInteract(PlayerInteractor player)
    {
        SystemNotify.instance.SendNotify($"Mua nhà", "Bạn có chắc muốn mua căn nhà này với giá {asd}", BuyHouse, () => { player.QuitInteracting(); });
    }

    public void ShowPrompt()
    {
        
    }

    void BuyHouse()
    {
        if (HouseManager.instance.BuyHouse(GetComponentInParent<House>()))
        {
            this.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
