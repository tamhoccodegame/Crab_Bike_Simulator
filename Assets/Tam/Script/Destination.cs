using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public CrabService crabService;
    public bool isThisPickUpPoint;
    public GameObject customer;
    public List<GameObject> customerVisual = new List<GameObject>();
    private int currentCustomerIndex;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform c in customer.transform)
        {
            if (c.name == "Root") continue;
            customerVisual.Add(c.gameObject);
        }
        gameObject.SetActive(false);
    }

    private void OnEnable() 
    {
        if (customerVisual.Count == 0) return;
        isThisPickUpPoint = Vector3.Distance(crabService.currentPickUpPoint.position, transform.position) <= 0.5f;
        customerVisual[currentCustomerIndex].gameObject.SetActive(false);
        currentCustomerIndex = Random.Range(0, customerVisual.Count);
        Debug.Log(customerVisual.Count);
        customerVisual[currentCustomerIndex].gameObject.SetActive(isThisPickUpPoint);
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
                customerVisual[currentCustomerIndex].gameObject.SetActive(false);
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
            }
            gameObject.SetActive(false);
        }
    }
}
