using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CarInShop : MonoBehaviour, IInteractable
{
    public GameObject informPanelPrefab;
    public GameObject carToDrivePrefab;
    private GameObject informPanel;
    private PlayerInteractor currentPlayer;

    public string carName;
    public int carPrice;

    public Action<CarInShop> onCarBought;
    private Camera cam;

    public KeyCode keyToInteract => KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        informPanel = Instantiate(informPanelPrefab);
        informPanel.transform.SetParent(transform, true);
        informPanel.transform.position = transform.position;
        informPanel.transform.localPosition = informPanelPrefab.transform.position;
        informPanel.transform.Find("Panel").Find("Price").GetComponent<TextMeshProUGUI>().text = carPrice.ToString();
        informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(informPanel != null && cam != null)
        {
            Vector3 direction = cam.transform.position - informPanel.transform.position;
            direction.y = 0;
            informPanel.transform.rotation = Quaternion.LookRotation(-direction);
        }

        if(currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            SystemNotify.instance.SendNotify("Mua xe", $"Bạn có chắc muốn mua chiếc xe này với giá {carPrice} không?", BuyACar, () => { });
        }
    }

    public void BuyACar()
    {
        if (!PlayerCash.instance.CostMoney(carPrice)) return;
        Debug.Log("Bought A Car");
        //Logic Mua xe
        informPanel.SetActive(false);
        StopAllCoroutines();
        onCarBought?.Invoke(this);
    }

    public void OnInteract(PlayerInteractor player)
    {
        StartCoroutine(SetPlayer(player));
    }

    IEnumerator SetPlayer(PlayerInteractor player)
    {
        yield return null;
        currentPlayer = player;
        yield return new WaitForSeconds(3f);
        currentPlayer = null;
        informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(false);
    }

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
    }

    public void ResetInteractState()
    {
        currentPlayer = null; 
    }
}
