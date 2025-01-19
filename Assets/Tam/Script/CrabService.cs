using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrabService : MonoBehaviour
{
    private CashSystem cashSystem;

    public List<Transform> destinationList;
    public Transform currentPickUpPoint;
    public Transform currentDropOffPoint;

    public bool isOnDuty;
    public bool isAcceptTrip;
    public bool isTripCompleted;

    public float customerCallCooldown;
    public float customerCallTimer;

    public Button dutyButton;
    public GameObject notificationPanel;

    public ArrowPointer directionArrow;

    // Start is called before the first frame update
    void Start()
    {
        cashSystem = new CashSystem();
        customerCallTimer = customerCallCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        SendNotification();
    }
    
    public void SendNotification()
    {
		if (isOnDuty && !isAcceptTrip && customerCallTimer > 0 && !notificationPanel.activeSelf)
		{
			customerCallTimer -= Time.deltaTime;
		}
		if (customerCallTimer <= 0)
		{
            GetDestination();
            cashSystem.CalculatePayment(currentPickUpPoint.position, currentDropOffPoint.position);
			//Gửi thông báo có khách
			customerCallTimer = customerCallCooldown;
			notificationPanel.SetActive(true);
            TextMeshProUGUI text = notificationPanel.transform.Find("Destination")
                                       .GetComponent<TextMeshProUGUI>();
            text.text = $"{currentPickUpPoint.name} => {currentDropOffPoint.name} ({cashSystem.currentPayment} VND)";
        }
	}

    public int GetPayment()
    {
        return (int)cashSystem.currentPayment;
    }

    public void GetDestination()
    {
        Transform pickUpPoint = destinationList[Random.Range(0, destinationList.Count - 1)];
		
        currentPickUpPoint = pickUpPoint;

        Transform dropOffPoint;
        do
        {
            dropOffPoint = destinationList[Random.Range(0, destinationList.Count - 1)];
        }
        while (dropOffPoint == pickUpPoint);


        currentDropOffPoint = dropOffPoint;
	}

    public void ChangeDuty()
    {
        if (isOnDuty)
        {
			isOnDuty = false;
			dutyButton.GetComponent<Image>().color = Color.red;
		}
        else
        {
			isOnDuty = true;
			dutyButton.GetComponent<Image>().color = Color.green;
		}
    }

    public void AcceptTrip()
    {
        if(!isOnDuty)
        {
            Debug.LogError("You must be on duty first");
        }
        else
        {
            directionArrow.gameObject.SetActive(true);
            isAcceptTrip = true;
            directionArrow.checkpoint = currentPickUpPoint;
            currentPickUpPoint.gameObject.SetActive(true);
            isTripCompleted = false;
        }

        //Xử lý tạo GPS trên map
    }

    public void CancelTrip()
    {
        currentPickUpPoint.gameObject.SetActive(false);
        currentDropOffPoint.gameObject.SetActive(false);
        currentPickUpPoint = null;
        currentDropOffPoint = null;
    }

    public void SetDestination()
    {
        directionArrow.checkpoint = currentDropOffPoint;
        currentDropOffPoint.gameObject.SetActive(true);
    }

    public void CompleteTrip()
    {
        directionArrow.gameObject.SetActive(false);
        isAcceptTrip = false;
        isTripCompleted = true;
        currentPickUpPoint.gameObject.SetActive(false);
        currentDropOffPoint.gameObject.SetActive(false);
        currentPickUpPoint = null;
        currentDropOffPoint = null;
        directionArrow.checkpoint = null;

        //Cộng tiền cho player
    }
}
