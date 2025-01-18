using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour, IInteractable
{
    public Transform sitPosition;
    private PlayerInteractor currentPlayer;
    public Transform leftHand;
    public Transform rightHand;
    public GameObject parent;
    public bool isInputCoolDown = false;

    public void OnInteract(PlayerInteractor player)
    {
        currentPlayer = player;
        currentPlayer.GetComponent<TPlayerController>().enabled = false;
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.transform.position = sitPosition.position;
        currentPlayer.transform.rotation = sitPosition.rotation;
        currentPlayer.transform.SetParent(sitPosition.transform, true);
        currentPlayer.GetComponent<Animator>().SetBool("Riding", true);
        currentPlayer.GetComponent<PlayerInteractor>().enabled = false;
        currentPlayer.GetComponent<IKHandler>().leftHandTarget = leftHand;
        currentPlayer.GetComponent<IKHandler>().rightHandTarget = rightHand;
        parent.GetComponent<BikeController>().enabled = true;
        StartCoroutine(InputCooldown());
    }

    IEnumerator InputCooldown()
    {
        isInputCoolDown = true;
        yield return null;
        isInputCoolDown = false;
    }

    public void ShowPrompt()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInputCoolDown) return;
        if (Input.GetKeyDown(KeyCode.F) && currentPlayer != null)
        {
            currentPlayer.transform.position = sitPosition.position + new Vector3(0, 0, -3);
            currentPlayer.transform.SetParent(null);
            currentPlayer.GetComponent<TPlayerController>().enabled = true;
            currentPlayer.GetComponent<CharacterController>().enabled = true;
            currentPlayer.GetComponent<Animator>().SetBool("Riding", false);
            parent.GetComponent<BikeController>().enabled = false;
            currentPlayer.GetComponent<PlayerInteractor>().enabled = true;
            currentPlayer.GetComponent<IKHandler>().leftHandTarget = null;
            currentPlayer.GetComponent<IKHandler>().rightHandTarget = null;
            currentPlayer.currentInteractable = null;
            currentPlayer = null;
        }
    }
}
