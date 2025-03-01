using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 3f;
            Collider [] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent(out NPCInteractable npcInteractable) && npcInteractable != null)
                {
                    //npcInteractable.Interact(transform);

                    if (npcInteractable != null)  // 🔹 Kiểm tra NULL trước khi gọi Interact()
                    {
                        Debug.Log("Interacting with NPC: " + npcInteractable.gameObject.name);
                        npcInteractable.Interact(transform);
                    }
                    else
                    {
                        Debug.LogWarning("NPCInteractable is NULL!");
                    }
                }
            }
        }
    }


    public NPCInteractable GetInteractableObject()
    {
        List<NPCInteractable> npcInteractableList = new List<NPCInteractable>();    
        float interactRange = 3f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach(Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractableList.Add(npcInteractable);
                //return npcInteractable;
            }
        }

        NPCInteractable closetNPCInteractable = null;
        foreach(NPCInteractable npcInteractable in npcInteractableList)
        {
            if (closetNPCInteractable == null)
            {
                closetNPCInteractable = npcInteractable;
            }
            else
            {
                if(Vector3.Distance(transform.position, npcInteractable.transform.position)<
                    Vector3.Distance(transform.position, closetNPCInteractable.transform.position))
                {
                    //Closer
                    closetNPCInteractable = npcInteractable;
                }
            }
        }
        
        
        return closetNPCInteractable;
    }
}
