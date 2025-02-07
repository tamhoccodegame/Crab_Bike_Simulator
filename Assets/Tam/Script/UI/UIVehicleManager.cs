using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIVehicleManager : MonoBehaviour
{
    private VehicleManager vehicleManager;

    public Transform vehicleIconContainer;
    public Transform vehicleIconTemplate;

    // Start is called before the first frame update
    void Start()
    {
        
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
            rectTransform.Find("CarPrice").GetComponent<TextMeshProUGUI>().text = vehicle.price.ToString();

            //Đăng ký sự kiện cho các Button để spawn xe raaaa, cất xeee, bán xeee
        }
    }
}
