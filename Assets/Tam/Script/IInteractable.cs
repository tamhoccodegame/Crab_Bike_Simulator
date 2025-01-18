using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void ShowPrompt();
    void OnInteract(PlayerInteractor player);
    void OnExit();
}
