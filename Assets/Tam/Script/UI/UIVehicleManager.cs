using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVehicleManager : MonoBehaviour
{
    private VehicleManager vehicleManager;

    public Transform vehicleIconContainer;
    public Transform vehicleIconTemplate;
    public Button depositCarButton;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onSceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(SaveData obj)
    {
        playerPosition = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVehicleManager(VehicleManager _vehicleManager)
    {
        vehicleManager = _vehicleManager;
        vehicleManager.onVehicleChange += RefreshUI;
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach(Transform child in vehicleIconContainer)
        {
            if (child == vehicleIconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(GameObject child in vehicleManager.ownVehicles)
        {
            Vehicle vehicle = child.GetComponent<Vehicle>();
            if (vehicle == null)
            {
                Debug.LogError("No Vehicle Component Found!!");
                continue;
            }
            RectTransform rectTransform = Instantiate(vehicleIconTemplate, vehicleIconContainer).GetComponent<RectTransform>();
            rectTransform.gameObject.SetActive(true);
            rectTransform.Find("CarName").GetComponent<TextMeshProUGUI>().text = vehicle.name.ToString();
            rectTransform.Find("CarPrice").GetComponent<TextMeshProUGUI>().text = "Giá: " + vehicle.price.ToString();
            rectTransform.Find("CarSpeed").GetComponent<TextMeshProUGUI>().text = "Tốc độ: " + vehicle.speed.ToString();

            //Đăng ký sự kiện cho các Button để spawn xe raaaa, cất xeee, bán xeee
            rectTransform.Find("CallSpawnCarBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Vector3 spawnPosition = playerPosition.position + playerPosition.forward * 3f;
                vehicleManager.DeliVehicle(child, spawnPosition);
            });

            rectTransform.Find("SellCarBtn").GetComponent<Button>().onClick.AddListener(() =>
            {

            });
        }

        if (vehicleManager.HasActiveVehicle())
        {
            depositCarButton.gameObject.SetActive(true);
            depositCarButton.onClick.AddListener(() => { vehicleManager.DepositVehicle(); });
        }
        else
        {
            depositCarButton.onClick.RemoveAllListeners();
            depositCarButton.gameObject.SetActive(false);
        }
    }
}
