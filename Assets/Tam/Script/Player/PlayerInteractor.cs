using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        Camera cam = Camera.main;
        float rayHeight = playerVisual.GetComponent<Renderer>().bounds.size.y / 2;
        Vector3 offset = new Vector3(0, rayHeight, 0);
        Ray ray = new Ray(playerVisual.position + offset, cam.transform.forward);

        
            if (Physics.Raycast(ray, out hit, 3f))
            {

                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    interactable.ShowPrompt(this);
                }
            }
            else if(currentInteractable != null) 
            {
                currentInteractable.ResetInteractState();
                currentInteractable = null;
            }
        
       
        Debug.DrawRay(playerVisual.position + offset, cam.transform.forward * 3f, Color.red);
    }

}
