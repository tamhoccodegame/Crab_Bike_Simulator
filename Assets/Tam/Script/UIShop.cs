using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour, IInteractable
{
    public GameObject shopUI;
    private IShop shop;
    public Transform shopItemTemplate;
    public Transform shopItemContainer;
    public Button buyButton;

    private PlayerInteractor currentPlayer;
    private Inventory playerInventory;
    private PlayerCash playerCash;

    public void OnExit()
    {
        shopUI.SetActive(false);
        currentPlayer.GetComponent<Animator>().SetBool("Shopping", false);
        currentPlayer = null;
    }

    public void OnInteract(PlayerInteractor player)
    {
        shopUI.SetActive(true);
        currentPlayer = player;
        currentPlayer.GetComponent<Animator>().SetBool("Shopping", true);
    }

    public void ShowPrompt()
    {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        shop = GetComponent<IShop>();

        RefreshShopUI();
    }

    public void SetPlayerInventory(Inventory _playerInventory)
    {
        playerInventory = _playerInventory;
    }

    public void SetPlayerCash(PlayerCash _playerCash)
    {
        playerCash = _playerCash;
    }

    void RefreshShopUI()
    {
        foreach(Transform child in shopItemContainer)
        {
            if(child == shopItemTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }


        foreach (IShopItem shopItem in shop.GetShopItems())
        {
            RectTransform itemRectTransform = Instantiate(shopItemTemplate, shopItemContainer).GetComponent<RectTransform>();
            itemRectTransform.gameObject.SetActive(true);
            itemRectTransform.GetComponent<Image>().sprite = shopItem.GetSprite();
            itemRectTransform.Find("Price").GetComponent<TextMeshProUGUI>().text = shopItem.GetPrice().ToString();
            itemRectTransform.GetComponent<Button>().onClick.RemoveAllListeners();

            buyButton.onClick.RemoveAllListeners();
            itemRectTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                buyButton.gameObject.SetActive(true);
                buyButton.onClick.AddListener(() =>
                {
                    Debug.Log($"Buy {shopItem.GetSprite().name}");
                    if(playerCash.CostMoney(shopItem.GetPrice()))
                    playerInventory.AddItem(shopItem);
                    else
                    {
                        Debug.Log("Không đủ tiền");
                    }
                });
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
