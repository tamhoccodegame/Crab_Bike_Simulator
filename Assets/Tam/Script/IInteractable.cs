using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void ShowPrompt(PlayerInteractor player);

    void ResetInteractState();
    KeyCode keyToInteract { get; }
}
