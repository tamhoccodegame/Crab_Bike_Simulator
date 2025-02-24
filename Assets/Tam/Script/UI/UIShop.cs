using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public static UIShop instance;
    public GameObject shopUI;
    private IShop shop;
    public Transform shopItemTemplate;
    public Transform shopItemContainer;
    public Button buyButton;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetIShop(IShop _shop)
    {
        shop = _shop;
    }

    public void ShowShop()
    {
        shopUI.SetActive(true);
        GameManager.instance.ChangeGameState(GameManager.GameState.Menu);
        RefreshShopUI();
    }

    public void HideShop()
    {
        shopUI.SetActive(false);
        GameManager.instance.ChangeGameState(GameManager.GameState.Playing);
    }

    public bool IsShopUIActive()
    {
        return shopUI.activeSelf;
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
                    if(PlayerCash.instance.CostMoney(shopItem.GetPrice()))
                    PlayerInventory.instance.AddItem(shopItem);
                    else
                    {
                        Debug.Log("Không đủ tiền");
                    }
                });
            });
        }

    }
}
