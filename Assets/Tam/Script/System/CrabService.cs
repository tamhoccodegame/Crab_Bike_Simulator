using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine.Utility;

public class CrabService : MonoBehaviour
{
    public static CrabService instance;

    public Transform player;
    private CashSystem cashSystem;
    private PlayerCash playerCash;


    public List<Transform> destinationList;
    public Vector3 currentPickUpPosition;
    public Transform currentDropOffPoint;

    public bool isOnDuty;

    public Button dutyButton;
    public GameObject notificationPanel;

    public ArrowPointer directionArrow;
    public Slider progressTripSlider;

    private Vector3 currentDestination;
    private float tripLong;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cashSystem = new CashSystem();
        playerCash = FindObjectOfType<PlayerCash>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTripProgress();
    }

    void UpdateTripProgress()
    {
        if (!progressTripSlider.gameObject.activeSelf) return;


        float currentDistance = Vector3.Distance(player.position, currentDestination);

        float sliderValue = 1 - (currentDistance / tripLong);

        progressTripSlider.value = sliderValue;
    }


    public bool TryPingTrip(Vector3 pickUpPosition)
    {
        if(isOnDuty && !notificationPanel.activeSelf)
        {
            PingTrip(pickUpPosition);
            return true;
        }
        CustomerBookCrab.ResetBookCrab();
        return false;
    }

    private void PingTrip(Vector3 pickUpPosition)
    {
        currentPickUpPosition = pickUpPosition;
        GetDestination();
        cashSystem.CalculatePayment(currentPickUpPosition, currentDropOffPoint.position);
        float distance = Vector3.Distance(player.position, currentPickUpPosition);
        //Gửi thông báo có khách
        notificationPanel.SetActive(true);
        notificationPanel.transform.Find("PickUp")
                                   .GetComponent<TextMeshProUGUI>().text = $"Điểm đón: {currentPickUpPosition} ({(int)(distance / 100)}km)";
        notificationPanel.transform.Find("DropOff")
                                   .GetComponent<TextMeshProUGUI>().text = $"Điểm đến: {currentDropOffPoint.name}";
        notificationPanel.transform.Find("Price")
                                   .GetComponent<TextMeshProUGUI>().text = $"Phí cuốc: {cashSystem.currentPayment.ToString("N0")} VND";
    }

    public void GetDestination()
    {
        Transform dropOffPoint;
        {
            dropOffPoint = destinationList[Random.Range(0, destinationList.Count - 1)];
        }
        currentDropOffPoint = dropOffPoint;
    }

    public void ChangeDuty()
    {
        if (isOnDuty)
        {
            isOnDuty = false;
            dutyButton.GetComponent<Image>().color = Color.red;
            CancelTrip();
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
            currentDestination = currentPickUpPosition;
            tripLong = Vector3.Distance(player.position, currentDestination);
            directionArrow.gameObject.SetActive(true);
            directionArrow.checkpoint = currentPickUpPosition;
        }

    }

    public void CancelTrip()
    {
        if (currentPickUpPosition == null || currentDropOffPoint == null) return;
        currentDropOffPoint.gameObject.SetActive(false);
        currentDropOffPoint = null;
        CustomerBookCrab.ResetBookCrab();
    }

    public void SetDestination()
    {
        directionArrow.checkpoint = currentDropOffPoint.position;
        currentDestination = currentDropOffPoint.position;
        tripLong = Vector3.Distance(player.position, currentDestination);
        currentDropOffPoint.gameObject.SetActive(true);
    }

    public void CompleteTrip()
    {
        directionArrow.gameObject.SetActive(false);
        currentDropOffPoint.gameObject.SetActive(false);
        currentDropOffPoint = null;
        progressTripSlider.gameObject.SetActive(false);
        CustomerBookCrab.ResetBookCrab();
        //Cộng tiền cho player
        playerCash.AddMoney((int)cashSystem.currentPayment);
    }
}
