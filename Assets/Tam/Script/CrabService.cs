using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrabService : MonoBehaviour
{
    private CashSystem cashSystem;

    public List<Transform> destinationList;
    private Transform currentPickUpPoint;
    private Transform currentDropOffPoint;

    public bool isOnDuty;
    public bool isOnTrip;

    public float customerCallCooldown;
    public float customerCallTimer;

    public Button dutyButton;
    public GameObject notificationPanel;

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
		if (isOnDuty && !isOnTrip && customerCallTimer > 0 && !notificationPanel.activeSelf)
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

    public void GetDestination()
    {
        Transform pickUpPoint = destinationList[0];
		float minDistance = Vector3.Distance(transform.position, destinationList[0].position);

		for (int i = 1; i < destinationList.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, destinationList[i].position);

			if (distance < minDistance)
            {
                minDistance = distance;
                pickUpPoint = destinationList[i];
            }
        }
        currentPickUpPoint = pickUpPoint;

        Transform dropOffPoint = destinationList[0];
        float maxDistance = Vector3.Distance(pickUpPoint.position, dropOffPoint.position);

        if(maxDistance <= 10f)
        {
			for (int i = 1; i < destinationList.Count; i++)
			{
				float distance = Vector3.Distance(pickUpPoint.position, destinationList[i].position);
                if (distance > 10) break;
				if (distance > maxDistance)
				{
					maxDistance = distance;
					dropOffPoint = destinationList[i];
				}
			}
		}

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
            isOnTrip = true;
        }

        //Xử lý tạo GPS trên map
    }

    public void CompleteTrip()
    {
        isOnTrip = false;
        //Cộng tiền cho player
    }
}
