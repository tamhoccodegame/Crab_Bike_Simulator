using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementChecker : MonoBehaviour
{
    public ObjectPlacement objectPlacement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        objectPlacement.canPlace = false;
    }

    private void OnTriggerExit(Collider other)
    {
        objectPlacement.canPlace = true;
    }
}
