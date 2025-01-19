using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public CrabService crabService;
    public bool isThisPickUpPoint;
    public GameObject customer;
    private PlayerCash playerCash;

    // Start is called before the first frame update
    void Start()
    {
        playerCash = FindObjectOfType<PlayerCash>();
    }

    private void OnEnable() 
    {
        isThisPickUpPoint = Vector3.Distance(crabService.currentPickUpPoint.position, transform.position) <= 0.5f;
        customer.gameObject.SetActive(isThisPickUpPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BikeBody"))
        {
            if(isThisPickUpPoint)
            {
                crabService.SetDestination();
                Transform customerSitPosition = other.transform.Find("CustomerSitPosition");
                Transform duplicatCustomer = Instantiate(customer, customer.transform.parent).transform;
                customer.SetActive(false);
                duplicatCustomer.GetComponent<Animator>().SetBool("isOnTrip", true);
                duplicatCustomer.GetComponent<WaitingCustomer>().enabled = false;
                duplicatCustomer.transform.SetParent(customerSitPosition, true);
                duplicatCustomer.transform.localPosition = Vector3.zero;
                duplicatCustomer.transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else
            { 
                crabService.CompleteTrip();
                Transform customerSitPosition = other.transform.Find("CustomerSitPosition");
                Transform _customer = customerSitPosition.Find("NPC(Clone)");
                _customer.transform.SetParent(null);
                _customer.transform.position += new Vector3(0, 0, 2);
                _customer.transform.rotation = Quaternion.Euler(0,0,0);
                _customer.GetComponent<Animator>().SetBool("isOnTrip", false);
                _customer.GetComponent <WaitingCustomer>().enabled = true;
                playerCash.AddMoney(crabService.GetPayment());
            }
            gameObject.SetActive(false);
        }
    }
}
