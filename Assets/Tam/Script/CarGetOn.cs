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

    public KeyCode keyToInteract => KeyCode.F;

    public GameObject prompt;


    // Start is called before the first frame update
    void Start()
    {
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (prompt != null && prompt.activeSelf)
        {
            Vector3 direction = Camera.main.transform.position - prompt.transform.position;
            direction.y = 0; // Giữ nguyên trục Y để không bị nghiêng
            prompt.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (hasPlayer) prompt.SetActive(false);

        if (Input.GetKeyDown(keyToInteract))
        {
            if (!hasPlayer && currentPlayer != null)
            {
                EnterCar();
                hasPlayer = true;
                return;
            }
            else if (hasPlayer)
            {
                ExitCar();
                return;
            }
        }

        if(parent == null || !hasPlayer) return;

        if (parent.GetComponent<BikeController>().currentVelocityOffset > 0.01f)
        {
            currentPlayer.GetComponent<Animator>().SetBool("Stopping", false);
        }
        else
        {
            currentPlayer.GetComponent<Animator>().SetBool("Stopping", true);
        }

    }

    public void EnterCar()
    {
        motorSound.Play();
        currentPlayer.GetComponent<TPlayerController>().enabled = false;
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.transform.SetParent(sitPosition.transform, true);
        currentPlayer.transform.position = sitPosition.position;
        currentPlayer.transform.rotation = sitPosition.rotation;
        currentPlayer.GetComponent<Animator>().SetLayerWeight(1, 1);
        currentPlayer.GetComponent<IKHandler>().leftHandTarget = leftHand;
        currentPlayer.GetComponent<IKHandler>().rightHandTarget = rightHand;

        if (parent != null)
            parent.GetComponent<BaseCarController>().enabled = true;
        else GetComponent<BaseCarController>().enabled = true;

        currentPlayer.enabled = false;
    }


    public void ExitCar()
    {
        motorSound.Stop();
        hasPlayer = false;
        currentPlayer.enabled = true;
        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position += -currentPlayer.transform.right * 1.5f;
        currentPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);
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

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
        hasPlayer = false;
        prompt.SetActive(true);
    }

    public void ResetInteractState()
    {
        currentPlayer = null;
        prompt.SetActive(false);
    }
}
