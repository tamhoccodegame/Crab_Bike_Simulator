using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerUIGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(NPCInteractable npcInteractable)
    {
        containerUIGameObject.SetActive(true);
        interactText.text = npcInteractable.GetInteractText();
    }

    private void Hide()
    {
        containerUIGameObject.SetActive(false);
    }
}
