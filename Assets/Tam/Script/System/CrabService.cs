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
    public CashSystem cashSystem;
    private PlayerCash playerCash;

    public string[] customersName;

    private System.Action onTripAccepted;
    private System.Action onTripDenied;

    public List<Transform> destinationList;
    public Vector3 currentPickUpPosition;
    public Transform currentDropOffPoint;

    public bool isOnDuty;

    public Button dutyButton;
    public GameObject notificationPanel;

    public RectTransform arrowUI;
    public Slider progressTripSlider;
    public GameObject minimap;
    public Slider m_progressTripSlider;

    private Vector3 currentDestination;
    private float tripLong;

    //[Header("RatingTab")]
    //public TextMeshProUGUI ratingStar;
    //public Transform ratingBoxTemplate;
    //public Transform ratingBoxContainer;

    [Header("ProfileTab")]
    private int rideCount;
    private int totalRide;
    private float kmCount;
    public TextMeshProUGUI rideCountText;
    public TextMeshProUGUI kmCountText;
    public TextMeshProUGUI acceptRateText;

    private bool isOnTrip = false;  

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cashSystem = new CashSystem();
        playerCash = PlayerCash.instance;
        player = GameObject.FindWithTag("Player").transform;
        rideCount = 0;
        totalRide = 0;
        kmCount = 0;

        GameManager.instance.onScenePreLoad += OnScenePreLoad;
        GameManager.instance.onSceneLoaded += OnSceneLoaded;
    }

    private void OnScenePreLoad()
    {
        player = null;
    }

    private void OnSceneLoaded(SaveData data)
    {
        playerCash = PlayerCash.instance;
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;
        if(isOnDuty && !GameManager.instance.phoneUI.activeSelf)
        {
            minimap.SetActive(true);
        }
        else
        {
            minimap.SetActive(false);
        }
        UpdateArrowUI();
        UpdateTripProgress();
    }

    void UpdateArrowUI()
    {
        if (currentDestination != null)
        {
            Vector3 direction = currentDestination - player.position;
            Vector3 localDirection = player.InverseTransformDirection(direction);

            float angle = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    void UpdateTripProgress()
    {
        if (!progressTripSlider.gameObject.activeSelf) return;


        float currentDistance = Vector3.Distance(player.position, currentDestination);

        float sliderValue = 1 - (currentDistance / tripLong);

        progressTripSlider.value = sliderValue;

        m_progressTripSlider.value = sliderValue;
    }


    public bool TryPingTrip(Vector3 pickUpPosition, System.Action _onTripAccepted, System.Action _onTripDenied)
    {
        if(!notificationPanel.activeSelf && !isOnTrip)
        {
            PingTrip(pickUpPosition);
            totalRide++;
            onTripAccepted = _onTripAccepted;
            onTripDenied = _onTripDenied;
            isOnTrip = true;
            return true;
        }
        Debug.Log("Called by Complete TryPingTrip");
        CustomerBookCrab.SetBooking(true);
        return false;
    }

    private void PingTrip(Vector3 pickUpPosition)
    {
        if (!GameManager.instance.phoneUI.activeSelf)
        {
            SystemNotify.instance.SendMNotify("Bạn có một thông báo mới [P]", "Có một khách hàng đang chờ bạn");
        }

        currentPickUpPosition = pickUpPosition;
        GetDestination();
        cashSystem.CalculatePayment(currentPickUpPosition, currentDropOffPoint.position);
        float distance = Vector3.Distance(player.position, currentPickUpPosition);
        //Gửi thông báo có khách
        notificationPanel.SetActive(true);
        string customerName = customersName[Random.Range(0, customersName.Length)];
        notificationPanel.transform.Find("PickUp")
                                   .GetComponent<TextMeshProUGUI>().text = $"Điểm đón: {customerName} ({(int)(distance / 100)}km)";
        notificationPanel.transform.Find("DropOff")
                                   .GetComponent<TextMeshProUGUI>().text = $"Điểm đến: {currentDropOffPoint.name}";
        notificationPanel.transform.Find("Price")
                                   .GetComponent<TextMeshProUGUI>().text = $"Phí cuốc: {cashSystem.currentPayment.ToString("N0")} VND";
    }

    public void GetDestination()
    {
        Transform dropOffPoint;
        {
            dropOffPoint = destinationList[Random.Range(0, destinationList.Count)];
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
            onTripAccepted?.Invoke();
            onTripAccepted = null;
            progressTripSlider.gameObject.SetActive(true);
            currentDestination = currentPickUpPosition;
            tripLong = Vector3.Distance(player.position, currentDestination);
            arrowUI.gameObject.SetActive(true);
        }

    }

    public void CancelTrip()
    {
        if (currentPickUpPosition == null || currentDropOffPoint == null) return;
        onTripAccepted = null;
        currentDropOffPoint.gameObject.SetActive(false);
        currentDropOffPoint = null;
        Debug.Log("Called by Cancel Trip");
        isOnTrip = false;
        onTripDenied?.Invoke();
        onTripDenied = null;
        CustomerBookCrab.SetBooking(false);
    }

    public void SetDestination()
    {
        currentDestination = currentDropOffPoint.position;
        tripLong = Vector3.Distance(player.position, currentDestination);
        currentDropOffPoint.gameObject.SetActive(true);
    }

    public void CompleteTrip()
    {
        rideCount++;
        kmCount += tripLong/100f;
        rideCountText.text = rideCount.ToString();
        kmCountText.text = kmCount.ToString("N0");
        acceptRateText.text = $"{(rideCount / totalRide) * 100}%";
        arrowUI.gameObject.SetActive(false);
        currentDropOffPoint.gameObject.SetActive(false);
        currentDropOffPoint = null;
        progressTripSlider.gameObject.SetActive(false);
        isOnTrip = false;
        Debug.Log("Called by Complete Trip");
        CustomerBookCrab.SetBooking(false);
    }
}
