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
        informPanel = Instantiate(informPanelPrefab);
        informPanel.transform.SetParent(transform, true);
        informPanel.transform.position = transform.position;
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

        if (Input.GetKeyDown(KeyCode.F) && currentPlayer != null)
        {
            if (!PlayerCash.instance.CostMoney(interiorPrice)) return;
            Debug.Log("Bought A Interior");
            //Logic Mua xe
            informPanel.SetActive(false);
            StopAllCoroutines();
            currentPlayer.QuitInteracting();
            onInteriorBought?.Invoke(this);
        }
    }

    public void ShowPrompt()
    {

    }

    public void OnInteract(PlayerInteractor player)
    {
        player.QuitInteracting();
        informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(true);
        StartCoroutine(SetPlayer(player));
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
