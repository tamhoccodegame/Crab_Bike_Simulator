using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorShop : MonoBehaviour
{
    public Transform[] iSpawnPoints;
    public GameObject[] iPrefabs;
    public InteriorInShop[] interiorsInShop;

    // Start is called before the first frame update
    void Start()
    {
        interiorsInShop = new InteriorInShop[iSpawnPoints.Length];
        TrySpawnInterior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Được gọi mỗi 6h sáng
    [ContextMenu("SpawnInterior")]
    public void TrySpawnInterior()
    {
        for (int i = 0; i < iSpawnPoints.Length; i++)
        {
            if (iSpawnPoints[i].childCount > 0) continue;
            var selectedPrefab = iPrefabs[Random.Range(0, iPrefabs.Length)];
            var go = Instantiate(selectedPrefab, selectedPrefab.transform.position, selectedPrefab.transform.rotation);
            go.transform.SetParent(iSpawnPoints[i], true);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
            interiorsInShop[i] = go.GetComponent<InteriorInShop>();
            interiorsInShop[i].onInteriorBought = TryBuyInterior;
        }
    }

    //Sẽ được gọi bởi xe được mua
    public void TryBuyInterior(InteriorInShop _interiorInShop)
    {
        for (int i = 0; i < iSpawnPoints.Length; i++)
        {
            if (iSpawnPoints[i].childCount == 0) continue;
            GameObject child = iSpawnPoints[i].GetChild(0).gameObject;
            if (child == _interiorInShop.gameObject)
            {
                interiorsInShop[i].onInteriorBought -= TryBuyInterior;
                IShopItem iShop = new Furniture { type = _interiorInShop.furnitureType };
                Debug.Log(iShop);
                PlayerInventory.instance.AddItem(iShop);
                Destroy(child);
                break;
            }
        }
    }

    private GameObject GetPrefab(GameObject veh)
    {
        string nameToFind = veh.name.Replace("(Clone)", "").Trim();
        foreach (GameObject p in iPrefabs)
        {
            p.name = nameToFind;
            return p;
        }
        return null;
    }
}
