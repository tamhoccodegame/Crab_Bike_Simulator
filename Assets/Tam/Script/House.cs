using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public HouseDoor houseDoor;
    public int id;
    public bool isOwned = false;

    public int price;

    // Start is called before the first frame update
    void Start()
    {
        houseDoor.enabled = false;  
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("SellThisHouse")]
    public void Sell()
    {
        houseDoor.isOpen = false;
        houseDoor.isClosed = false;
        StartCoroutine(SellHouseCoroutine());
    }

    IEnumerator SellHouseCoroutine()
    {
        while (!houseDoor.isClosed)
        {
            yield return null;
        }
        HouseManager.instance.SellHouse();
    }

}
