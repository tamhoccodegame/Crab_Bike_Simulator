using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteriorInShop : MonoBehaviour, IInteractable
{
    public Furniture.FurnitureType furnitureType;
    public GameObject informPanelPrefab;
    private GameObject informPanel;
    private PlayerInteractor currentPlayer;

    public string interiorName;
    public int interiorPrice;

    public Action<InteriorInShop> onInteriorBought;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        informPanel = Instantiate(informPanelPrefab, transform.parent);
        informPanel.transform.localPosition = informPanelPrefab.transform.position;
        informPanel.transform.Find("Panel").Find("Price").GetComponent<TextMeshProUGUI>().text = interiorPrice.ToString();
        informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (informPanel != null && cam != null)
        {
            Vector3 direction = cam.transform.position - informPanel.transform.position;
            direction.y = 0;
            informPanel.transform.rotation = Quaternion.LookRotation(-direction);
        }
    }

    public void ShowPrompt()
    {

    }

    public void OnInteract(PlayerInteractor player)
    {
        currentPlayer = player;
        SystemNotify.instance.SendNotify("Mua nội thất", $"Bạn có muốn mua món đồ này với giá {interiorPrice} không?", BuyInterior, () => { currentPlayer.QuitInteracting(); });
    }

    void BuyInterior()
    {
        currentPlayer.QuitInteracting();
        informPanel.SetActive(false);
        currentPlayer = null;
        StopAllCoroutines();

        if (PlayerCash.instance.CostMoney(interiorPrice))
        {
            onInteriorBought?.Invoke(this);
        }
    }

    IEnumerator SetPlayer(PlayerInteractor player)
    {
        yield return null;
        currentPlayer = player;
        yield return new WaitForSeconds(3f);
        currentPlayer = null;
        informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(false);
        player.QuitInteracting();
    }
}
