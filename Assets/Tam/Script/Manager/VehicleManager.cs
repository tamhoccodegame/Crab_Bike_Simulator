using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager instance;
    public UIVehicleManager uIVehicleManager;
    public List<GameObject> ownVehicles; //Prefab của những chiếc xe có component Vehicle
    public List<GameObject> allVehiclesPrefab;
    public GameObject activeVehicle;
    public Action onVehicleChange;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        uIVehicleManager.SetVehicleManager(this);
        DeliVehicle(ownVehicles[0], PlayerInventory.instance.transform.position);
        GameManager.instance.onScenePreLoad += OnScenePreLoad;
        GameManager.instance.onSceneLoaded += OnSceneLoaded;
    }

    private void OnScenePreLoad()
    {
        DepositVehicle();
    }

    private void OnSceneLoaded(SaveData data)
    {
        uIVehicleManager.SetVehicleManager(this);

        ownVehicles.Clear();
        foreach (string vehicleName in data.ownVehiclesName)
        {
            GameObject prefab = allVehiclesPrefab.Find(v => v.name == vehicleName);
            if (prefab != null)
            {
                ownVehicles.Add(prefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCar(GameObject vehicle)
    {
        ownVehicles.Add(vehicle);
        onVehicleChange?.Invoke();
    }

    public void DeliVehicle(GameObject vehicle, Vector3 position)
    {
        if (!ownVehicles.Contains(vehicle)) return;
        if(activeVehicle != null)
        {
            Debug.Log("Hãy cất xe của bạn trước");
            return;
        }
        activeVehicle = Instantiate(vehicle, position, Quaternion.identity);
        onVehicleChange?.Invoke();
    }

    public void DepositVehicle()
    {
        if (activeVehicle == null)
        {
            Debug.Log("Có xe đâu mà gửi?");
            return;
        }
       
        Destroy(activeVehicle.gameObject);
        activeVehicle = null;
        onVehicleChange?.Invoke();
    }

    public bool HasActiveVehicle()
    {
        return activeVehicle != null;
    }
}
