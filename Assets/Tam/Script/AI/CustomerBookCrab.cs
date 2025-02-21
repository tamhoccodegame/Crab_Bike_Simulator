using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBookCrab : MonoBehaviour
{
    public GameObject destination;
    public float cooldown;
    public float timer;

    public static bool isBooking;

    public MonoBehaviour[] scripts;

    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;
        destination.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (isBooking || !CrabService.instance.isOnDuty) return;

        foreach (var script in scripts)
        {
            script.enabled = true;
        }


        if (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
        }    

        if(timer < 0)
        {
            if (CrabService.instance.TryPingTrip(transform.position, OnTripAccepted))
            {
                GetComponent<Animator>().SetBool("isWaiting", true);
                foreach(var script in scripts)
                {
                    script.enabled = false;
                }
                timer = cooldown;
                isBooking = true;
            }
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
