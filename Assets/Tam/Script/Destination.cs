using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public CrabService crabService;
    public bool isThisPickUpPoint;
    public GameObject customer;
    // Start is called before the first frame update
    void Start()
    {
        isThisPickUpPoint = Vector3.Distance(crabService.currentPickUpPoint.position, transform.position) <= 0.5f;
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
                customer.gameObject.SetActive(true);
                crabService.SetDestination();
                Transform customerSitPosition = other.transform.Find("CustomerSitPosition");
                customer.transform.position = customerSitPosition.position;
                customer.transform.rotation = customerSitPosition.rotation;
                customer.GetComponent<Animator>().SetBool("isOnTrip", true);
                customer.GetComponent<WaitingCustomer>().enabled = false;
                customer.transform.SetParent(customerSitPosition, true);
            }
            else
            {
                customer.gameObject.SetActive(false);
                crabService.CompleteTrip();
                Transform customerSitPosition = other.transform.Find("CustomerSitPosition");
                Transform _customer = customerSitPosition.Find("NPC");
                _customer.transform.SetParent(null);
                _customer.transform.position += new Vector3(0, 0, 2);
                _customer.GetComponent<Animator>().SetBool("isOnTrip", false);
                _customer.GetComponent <WaitingCustomer>().enabled = true;
            }
            gameObject.SetActive(false);
        }
    }
}
