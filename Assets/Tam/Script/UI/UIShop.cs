using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public GameObject shopUI;
    private IShop shop;
    public Transform shopItemTemplate;
    public Transform shopItemContainer;
    public Button buyButton;

    private PlayerInteractor currentPlayer;
    private Inventory playerInventory;


    public void OnExit()
    {
        shopUI.SetActive(false);
        currentPlayer.GetComponent<Animator>().SetBool("Shopping", false);
        currentPlayer = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteractor p = other.GetComponent<PlayerInteractor>();
        if(p != null)
        {
            currentPlayer = p;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteractor p = other.GetComponent<PlayerInteractor>();
        if (p != null && p == currentPlayer)
        {
            currentPlayer = null;
        }
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
        if(currentPlayer != null && Input.GetKeyDown(KeyCode.F))
        {
            TPlayerController.instance.ChangePlayerMode(shopUI.activeSelf ? TPlayerController.PlayerMode.Normal : TPlayerController.PlayerMode.Shopping);
            shopUI.SetActive(!shopUI.activeSelf);
            currentPlayer.GetComponent<Animator>().SetBool("Shopping", shopUI.activeSelf);
        }
    }
}
