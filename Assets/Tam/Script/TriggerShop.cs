using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShop : MonoBehaviour
{
    private PlayerInteractor currentPlayer;
    private IShop shop;

    public GameObject prompt;
    public KeyCode keyToInteract;

    // Start is called before the first frame update
    void Start()
    {
        shop = GetComponent<IShop>();
        if(prompt != null) prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            if (UIShop.instance.IsShopUIActive())
            {
                UIShop.instance.HideShop();
                UIShop.instance.SetIShop(null);
            }
            else
            {
                UIShop.instance.SetIShop(shop);
                UIShop.instance.ShowShop();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractor p = other.GetComponent<PlayerInteractor>();
        if (p != null)
        {
            currentPlayer = p;
            prompt.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractor p = other.GetComponent<PlayerInteractor>();
        if (p != null && p == currentPlayer)
        {
            currentPlayer = null;
            prompt.SetActive(false);
        }
    }

}
