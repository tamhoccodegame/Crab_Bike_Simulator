using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class CarInShop : MonoBehaviour, IInteractable
{
    private Vehicle vehicle;
    public GameObject informPanelPrefab;
    private GameObject informPanel;
    private PlayerInteractor currentPlayer;

    public Action<GameObject> onCarBought;
    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponent<Vehicle>();
        informPanel = Instantiate(informPanelPrefab);
        informPanel.transform.SetParent(transform, true);
        informPanel.transform.position = transform.position;
        informPanel.transform.localPosition = informPanelPrefab.transform.position;
        informPanel.transform.Find("Panel").Find("Price").GetComponent<TextMeshProUGUI>().text = vehicle.price.ToString("N0");
        informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(informPanel != null)
        {
            Vector3 direction = Camera.main.transform.position - informPanel.transform.position;
            direction.y = 0;
            informPanel.transform.rotation = Quaternion.LookRotation(-direction);
        }

        if(Input.GetKeyDown(KeyCode.F) && currentPlayer != null)
        {
            Debug.Log("Bought A Car");
            onCarBought?.Invoke(gameObject);
            //Logic Mua xe
            informPanel.transform.Find("Panel").Find("Confirm").gameObject.SetActive(false);
            StopAllCoroutines();
            currentPlayer.QuitInteracting();
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
