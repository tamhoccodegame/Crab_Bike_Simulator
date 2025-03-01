using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBookCrab : MonoBehaviour
{
    public GameObject destination;

    public static bool isBooking;

    // Start is called before the first frame update
    void Start()
    {
        destination.SetActive(false);
    }

    public void BookCrab()
    {
        if(isBooking || !CrabService.instance.isOnDuty) return;
        if (CrabService.instance.TryPingTrip(transform.position, OnTripAccepted))
        {
            GetComponent<Animator>().Play("Idle");
            GetComponent<Animation_Random>().enabled = true;
            GetComponent<CharacterNavigateController>().enabled = false;
            GetComponent<NPC_Behavior>().enabled = false;
            isBooking = true;
        }
    }

    void OnTripAccepted()
    {
        destination.SetActive(true);
    }

    public static void ResetBookCrab()
    {
        isBooking = false;
    }

}


