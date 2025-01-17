using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    RaycastHit hit;
    public IInteractable currentInteractable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, 10f))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if(interactable != null)
            {
                currentInteractable = interactable;
                interactable.ShowPrompt();
            }
        }

        if(currentInteractable != null && Input.GetKeyDown(KeyCode.F))
        {
            currentInteractable.OnInteract(this);
        }

        Debug.DrawLine(transform.position, transform.forward * 10, Color.red);
    }
}
