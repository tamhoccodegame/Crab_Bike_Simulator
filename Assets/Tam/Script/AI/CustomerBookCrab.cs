using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CustomerBookCrab : MonoBehaviour
{
    public GameObject destination;

    public static bool isBooking;
    private bool noPay;

    public float debt;


    // Start is called before the first frame update
    void Start()
    {
        destination.SetActive(false);
    }

    public void BookCrab()
    {
        if(isBooking || !CrabService.instance.isOnDuty) return;
        if (CrabService.instance.TryPingTrip(transform.position, OnTripAccepted, OnTripDenied))
        {
            noPay = Random.value < 0.5f;
            if(noPay) debt = (int)CrabService.instance.cashSystem.currentPayment;
            GetComponent<Animator>().Play("Idle");
            GetComponent<Animation_Random>().enabled = true;
            GetComponent<CharacterNavigateController>().enabled = false;
            GetComponent<NPC_Behavior>().enabled = false;
            GetComponent<NPC_Health>().enabled = false;
        }
    }
    public void CancelBookCrab()
    {

    }

    public void TryPayCash()
    {
        if(debt > 0)
        {
            return;
        }
        PlayerCash.instance.AddMoney((int)CrabService.instance.cashSystem.currentPayment);
        SystemNotify.instance.SendBigNoti($"+{((int)(CrabService.instance.cashSystem.currentPayment)).ToString("N0")}VND", Color.green);
        AudioManager.instance.PlaySound("Cash");
    }

    public void ResetDebt()
    {
        SystemNotify.instance.SendBigNoti($"+{((int)(CrabService.instance.cashSystem.currentPayment)).ToString("N0")}VND", Color.green);
        PlayerCash.instance.AddMoney((int)debt);
        AudioManager.instance.PlaySound("Cash");
        debt = 0;
    }

    void OnTripAccepted()
    {
        destination.SetActive(true);
    }

    void OnTripDenied()
    {
        destination.SetActive(false);
        GetComponent<Animator>().Play("Walking");
        GetComponent<Animation_Random>().enabled = false;
        GetComponent<CharacterNavigateController>().enabled = true;
        GetComponent<NPC_Behavior>().enabled = true;
        GetComponent<NPC_Health>().enabled = true;
    }

    public static void SetBooking(bool enable)
    {
        isBooking = enable;
    }

}


