using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour, IInteractable
{
    public Transform sitPosition;
    private bool hasPlayer = false;
    public GameObject prompt;

    public KeyCode keyToInteract => KeyCode.E;
    private PlayerInteractor currentPlayer;

    public void ResetInteractState()
    {
        currentPlayer = null;
        prompt.SetActive(false);
    }

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
        prompt.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(prompt != null) prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPlayer && prompt != null)
        {
            prompt.SetActive(false);
        }

        if(Input.GetKeyDown(keyToInteract))
        {
            if(!hasPlayer && currentPlayer != null)
            {
                Sit();
                return;
            }
            else if (hasPlayer)
            {
                StandUp();
                return;
            }
        }
    }

    public void Sit()
    {
        hasPlayer = true;
        currentPlayer.GetComponent<TPlayerController>().enabled = false;
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.GetComponent<Animator>().Play("Sitting");
        currentPlayer.transform.SetParent(sitPosition.transform, true);
        currentPlayer.transform.position = sitPosition.position;
        currentPlayer.transform.rotation = sitPosition.rotation;
        currentPlayer.enabled = false;
    }

    public void StandUp()
    {
        hasPlayer = false;
        currentPlayer.enabled = true;
        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position += -currentPlayer.transform.forward * 1.5f;
        currentPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentPlayer.GetComponent<TPlayerController>().enabled = true;
        currentPlayer.GetComponent<Animator>().Play("Idle");
        currentPlayer.GetComponent<CharacterController>().enabled = true;
        currentPlayer = null;
    }
}
