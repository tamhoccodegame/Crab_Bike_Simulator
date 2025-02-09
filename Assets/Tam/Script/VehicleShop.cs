using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleShop : MonoBehaviour
{
    public Transform[] vehSpawnPoints;
    public GameObject[] vehPrefabs;
    public CarInShop[] carsInShop;

    // Start is called before the first frame update
    void Start()
    {
        carsInShop = new CarInShop[vehSpawnPoints.Length];
        TrySpawnVeh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Được gọi mỗi 6h sáng
    [ContextMenu("SpawnCar")]
    public void TrySpawnVeh()
    {
        for (int i = 0; i < vehSpawnPoints.Length; i++)
        {
            if (vehSpawnPoints[i].childCount > 0) continue;
            var selectedPrefab = vehPrefabs[Random.Range(0, vehPrefabs.Length)];
            var go = Instantiate(selectedPrefab, selectedPrefab.transform.position, selectedPrefab.transform.rotation);
            go.transform.SetParent(vehSpawnPoints[i], true);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
            carsInShop[i] = go.GetComponent<CarInShop>();
            carsInShop[i].onCarBought = TryBuyVeh;
        }
    }

    //Sẽ được gọi bởi xe được mua
    public void TryBuyVeh(CarInShop carInShop)
    {
        for (int i = 0; i < vehSpawnPoints.Length; i++)
        {
            if (vehSpawnPoints[i].childCount == 0) continue;
            GameObject child = vehSpawnPoints[i].GetChild(0).gameObject;
            if (child == carInShop.gameObject)
            {
                carsInShop[i].onCarBought -= TryBuyVeh;
                VehicleManager.instance.AddCar(carInShop.carToDrivePrefab);
                Destroy(child);
                break;
            }
        }
    }

    private GameObject GetPrefab(GameObject veh)
    {
        string nameToFind = veh.name.Replace("(Clone)","").Trim();
        foreach (GameObject p in vehPrefabs)
        {
            p.name = nameToFind;
            return p;
        }
        return null;
    }
}
