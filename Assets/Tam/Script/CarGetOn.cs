using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGetOn : MonoBehaviour, IInteractable
{
    public Transform sitPosition;
    private PlayerInteractor currentPlayer;
    public Transform leftHand;
    public Transform rightHand;
    public GameObject parent;

    public AudioSource motorSound;

    private bool hasPlayer = false;

    public void OnInteract(PlayerInteractor player)
    {
        motorSound.Play();
        currentPlayer = player;
        currentPlayer.GetComponent<TPlayerController>().enabled = false;
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.transform.SetParent(sitPosition.transform, true);
        currentPlayer.transform.position = sitPosition.position;
        currentPlayer.transform.rotation = sitPosition.rotation;
        currentPlayer.GetComponent<Animator>().SetLayerWeight(1, 1);
        currentPlayer.GetComponent<IKHandler>().leftHandTarget = leftHand;
        currentPlayer.GetComponent<IKHandler>().rightHandTarget = rightHand;

        if(parent != null)
        parent.GetComponent<BaseCarController>().enabled = true;
        else GetComponent<BaseCarController>().enabled = true;

        currentPlayer.enabled = false;
        StartCoroutine(DelaySetPlayer());
    }

    IEnumerator DelaySetPlayer()
    {
        yield return null;
        hasPlayer = true;
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
        if (!hasPlayer) return;

        if (Input.GetKeyDown(KeyCode.F) && hasPlayer)
        {
            ExitCar();
        }

        if(parent == null) return;

        if (parent.GetComponent<BikeController>().currentVelocityOffset > 0.01f)
        {
            currentPlayer.GetComponent<Animator>().SetBool("Stopping", false);
        }
        else
        {
            currentPlayer.GetComponent<Animator>().SetBool("Stopping", true);
        }

    }


    public void ExitCar()
    {
        motorSound.Stop();
        hasPlayer = false;
        currentPlayer.enabled = true;
        currentPlayer.QuitInteracting();
        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position += -currentPlayer.transform.right * 1.5f;
        currentPlayer.GetComponent<TPlayerController>().enabled = true;
        currentPlayer.GetComponent<CharacterController>().enabled = true;

        if(parent != null)
        parent.GetComponent<BaseCarController>().enabled = false;
        else GetComponent<BaseCarController>().enabled = false;

        currentPlayer.GetComponent<Animator>().SetLayerWeight(1, 0);
        currentPlayer.GetComponent<IKHandler>().leftHandTarget = null;
        currentPlayer.GetComponent<IKHandler>().rightHandTarget = null;
        currentPlayer.currentInteractable = null;
        currentPlayer = null;
    }
}
