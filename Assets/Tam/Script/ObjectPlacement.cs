using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject objectToPlace;
    private Inventory inventory;
    private GameObject previewPlacement;
    public LayerMask groundLayer;
    public bool canPlace = false;

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
                SetTransparency(previewPlacement, 0.5f);
            }

            Collider previewCollider = previewPlacement.GetComponent<Collider>();
            Vector3 extents = previewCollider.bounds.extents;
            
            Vector3 colliderCenter = previewCollider.bounds.center;
            Vector3 offset = colliderCenter - previewCollider.transform.position;

            Vector3 placementPosition = hitInfo.point;
            previewPlacement.transform.position = placementPosition;
            Collider[] colliders = Physics.OverlapBox(placementPosition + offset, extents);

            if (colliders.Length > 1) canPlace = false;
            else if (colliders.Length > 0 && colliders[0].gameObject == previewPlacement.gameObject) canPlace = true;
       

            //previewPlacement.GetComponent<Renderer>().material.color = canPlace ? Color.green : Color.red;
            DrawDebugBox(placementPosition + offset, extents, canPlace ? Color.green : Color.red);

        }

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            Instantiate(objectToPlace, previewPlacement.transform.position, previewPlacement.transform.rotation);
            Destroy(previewPlacement.gameObject);
            previewPlacement = null;
            objectToPlace = null;
        }

        if (Input.GetKeyDown(KeyCode.R) && previewPlacement != null)
        {
            previewPlacement.transform.Rotate(0, 90, 0);
        }


    }


    private void DrawDebugBox(Vector3 center, Vector3 halfExtents, Color color)
    {
        Vector3[] corners = new Vector3[8];

        // Tính các góc của hộp
        corners[0] = center + new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z); // Bottom-Left-Back
        corners[1] = center + new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);  // Bottom-Right-Back
        corners[2] = center + new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z);  // Bottom-Left-Front
        corners[3] = center + new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z);   // Bottom-Right-Front
        corners[4] = center + new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);  // Top-Left-Back
        corners[5] = center + new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);   // Top-Right-Back
        corners[6] = center + new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z);   // Top-Left-Front
        corners[7] = center + new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);    // Top-Right-Front

        // Vẽ các cạnh của hộp
        Debug.DrawLine(corners[0], corners[1], color); // Bottom-Back
        Debug.DrawLine(corners[1], corners[3], color); // Bottom-Right
        Debug.DrawLine(corners[3], corners[2], color); // Bottom-Front
        Debug.DrawLine(corners[2], corners[0], color); // Bottom-Left

        Debug.DrawLine(corners[4], corners[5], color); // Top-Back
        Debug.DrawLine(corners[5], corners[7], color); // Top-Right
        Debug.DrawLine(corners[7], corners[6], color); // Top-Front
        Debug.DrawLine(corners[6], corners[4], color); // Top-Left

        Debug.DrawLine(corners[0], corners[4], color); // Left-Back
        Debug.DrawLine(corners[1], corners[5], color); // Right-Back
        Debug.DrawLine(corners[3], corners[7], color); // Right-Front
        Debug.DrawLine(corners[2], corners[6], color); // Left-Front
    }

    void SetTransparency(GameObject obj, float alpha)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
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
