using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject objectToPlace;
    private Inventory inventory;
    private GameObject previewPlacement;
    public LayerMask groundLayer;
    private bool canPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToPlace == null) return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.AddItem(previewPlacement.GetComponent<Furniture>());
            Destroy(previewPlacement.gameObject);
            objectToPlace = null;
            previewPlacement = null;
        }

        Vector3 rayOrigin = transform.position + transform.forward * 3;
        
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
        {
            if(previewPlacement == null && objectToPlace != null)
            {
                previewPlacement = Instantiate(objectToPlace);
            }

            Vector3 placementPosition = hitInfo.point;
            previewPlacement.transform.position = placementPosition;
        }
        Debug.DrawRay(rayOrigin, Vector3.down);
    }

    public void SetObjectToPlace(GameObject _objectToPlace)
    {
        objectToPlace = _objectToPlace;
    }

    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
    }
}
