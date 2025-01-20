using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine.Utility;

public class CrabService : MonoBehaviour
{
    public Transform player;
    private CashSystem cashSystem;
    private PlayerCash playerCash;


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
    public Slider progressTripSlider;

    private Vector3 currentDestination;
    private float tripLong;

    // Start is called before the first frame update
    void Start()
    {
        cashSystem = new CashSystem();
        playerCash = FindObjectOfType<PlayerCash>();
        customerCallTimer = customerCallCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTripProgress();
        SendNotification();
    }

    void UpdateTripProgress()
    {
        if (!progressTripSlider.gameObject.activeSelf) return;


        float currentDistance = Vector3.Distance(player.position, currentDestination);

        float sliderValue = 1 - (currentDistance / tripLong);

        progressTripSlider.value = sliderValue;
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
        if (!isOnDuty)
        {
            Debug.LogError("You must be on duty first");
        }
        else
        {
            progressTripSlider.gameObject.SetActive(true);
            currentDestination = currentPickUpPoint.position;
            tripLong = Vector3.Distance(player.position, currentDestination);
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
        currentDestination = currentDropOffPoint.position;
        tripLong = Vector3.Distance(player.position, currentDestination);
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
        progressTripSlider.gameObject.SetActive(false);
        //Cộng tiền cho player
        playerCash.AddMoney((int)cashSystem.currentPayment);
    }
}
