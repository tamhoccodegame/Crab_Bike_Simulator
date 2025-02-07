using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager instance;
    public UIVehicleManager uIVehicleManager;
    public GameObject[] ownVehicles; //Prefab của những chiếc xe có component Vehicle
    public GameObject activeVehicle;
    public Action onVehicleChange;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        uIVehicleManager.SetVehicleManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeliVehicle(GameObject vehicle, Vector3 position)
    {
        if (!ownVehicles.Contains(vehicle)) return;
        if(activeVehicle != null)
        {
            Debug.Log("Hãy cất xe của bạn trước");
            return;
        }
        Instantiate(vehicle, position, Quaternion.identity);
        activeVehicle = vehicle;
    }

    public void DepositVehicle()
    {
        Destroy(activeVehicle.gameObject);
        activeVehicle = null;
    }
}
