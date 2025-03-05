using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public static UIInventory instance;
    private Inventory inventory;
    public Transform itemSlotTemplate;
    public Transform itemSlotContainer;
    public Button useButton;
    public GameObject bagUI;


    private void Awake()
    {
        instance = this;
        bagUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshInventoryUI()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
            useButton.gameObject.SetActive(false);
        }


        foreach (IShopItem item in PlayerInventory.instance.GetItems())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.GetComponent<Image>().sprite = item.GetSprite();

            itemSlotRectTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            useButton.onClick.RemoveAllListeners();

            itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                useButton.gameObject.SetActive(true);
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() =>
                {
                    PlayerInventory.instance.UseItem(item);
                });
            });
        }
    }
}

