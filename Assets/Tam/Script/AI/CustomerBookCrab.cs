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
    }

    // Update is called once per frame
    void Update()
    {
        if (isBooking) return;

        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }    

        if(timer < 0)
        {
            if (CrabService.instance.TryPingTrip(transform.position))
            {
                isBooking = true;
                destination.SetActive(true);
                timer = cooldown;
            }
        }
    }

    public static void ResetBookCrab()
    {
        isBooking = false;
    }
}
