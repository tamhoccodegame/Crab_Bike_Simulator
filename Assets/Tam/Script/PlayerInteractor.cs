using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Transform playerVisual;
    RaycastHit hit;
    public IInteractable currentInteractable;
    private bool isInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerVisual.position, playerVisual.forward);

        if (!isInteracting)
        {
            if (Physics.Raycast(ray, out hit, 10f))
            {

                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    interactable.ShowPrompt();
                }
            }
            else
            {
                currentInteractable = null;
            }
        }
       

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.F))
        {
            if (!isInteracting)
            {
                currentInteractable.OnInteract(this);
                isInteracting = true;
            }
            else
            {
                currentInteractable.OnExit();
                isInteracting = false;
            }
        }

        Debug.DrawRay(playerVisual.position, playerVisual.forward * 10, Color.red);
    }
}
