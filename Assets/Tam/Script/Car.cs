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

    public void OnInteract(PlayerInteractor player)
    {
        currentPlayer = player;
        currentPlayer.GetComponent<TPlayerController>().enabled = false;
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.transform.SetParent(sitPosition.transform, true);
        currentPlayer.transform.position = sitPosition.position;
        currentPlayer.transform.rotation = sitPosition.rotation;
        currentPlayer.GetComponent<Animator>().SetLayerWeight(1, 1);
        currentPlayer.GetComponent<IKHandler>().leftHandTarget = leftHand;
        currentPlayer.GetComponent<IKHandler>().rightHandTarget = rightHand;
        parent.GetComponent<BikeController>().enabled = true;
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
        if (currentPlayer == null) return;
        if (parent.GetComponent<BikeController>().currentVelocityOffset > 0.01f)
        {
            currentPlayer.GetComponent<Animator>().SetBool("Stopping", false);
        }
        else
        {
            currentPlayer.GetComponent<Animator>().SetBool("Stopping", true);
        }
    }

    public void OnExit()
    {
        currentPlayer.transform.position = sitPosition.position + new Vector3(0, 0, -3);
        currentPlayer.transform.SetParent(null);
        currentPlayer.GetComponent<TPlayerController>().enabled = true;
        currentPlayer.GetComponent<CharacterController>().enabled = true;
        parent.GetComponent<BikeController>().enabled = false;
        currentPlayer.GetComponent<Animator>().SetLayerWeight(1, 0);
        currentPlayer.GetComponent<IKHandler>().leftHandTarget = null;
        currentPlayer.GetComponent<IKHandler>().rightHandTarget = null;
        currentPlayer.currentInteractable = null;
        currentPlayer = null;
    }
}
