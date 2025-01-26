using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarInShop : MonoBehaviour
{
    private Vehicle vehicle;
    public GameObject informPanelPrefab;
    private GameObject informPanel;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponent<Vehicle>();
        informPanel = Instantiate(informPanelPrefab);
        informPanel.transform.SetParent(transform, true);
        informPanel.transform.position = transform.position;
        informPanel.transform.localPosition = informPanelPrefab.transform.position;
        informPanel.transform.Find("Panel").Find("Price").GetComponent<TextMeshProUGUI>().text = vehicle.price.ToString("N0");
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
    }

}
