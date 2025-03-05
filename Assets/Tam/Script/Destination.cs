using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public bool isThisPickUpPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() 
    {

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
                CrabService.instance.SetDestination();
                Transform customerSitPosition = other.transform.Find("CustomerSitPosition");
                Transform customer = transform.parent;
                customer.GetComponent<CharacterController>().enabled = false;
                customer.GetComponent<Animator>().SetLayerWeight(2, 1);
                customer.GetComponent<Animation_Random>().enabled = false;
                customer.transform.SetParent(customerSitPosition, true);
                customer.transform.localPosition = Vector3.zero;
                customer.transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else
            { 
                CrabService.instance.CompleteTrip();
                Transform customerSitPosition = other.transform.Find("CustomerSitPosition");
                Transform _customer = customerSitPosition.GetChild(0);
                _customer.transform.SetParent(null);
                _customer.transform.position += new Vector3(0, 0, 2);
                _customer.transform.rotation = Quaternion.Euler(0,0,0);
                _customer.GetComponent<CustomerBookCrab>().TryPayCash();
                _customer.GetComponent<CharacterController>().enabled = true;
                _customer.GetComponent<NPC_Health>().enabled = true;
                _customer.GetComponent<Animator>().SetLayerWeight(2, 0);
                _customer.GetComponent<CharacterNavigateController>().enabled = true;
                _customer.GetComponent<NPC_Behavior>().enabled = true;
            }
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player"))
        {
            if (isThisPickUpPoint)
            {
                SystemNotify.instance.SendBigNoti("Bạn cần trên xe để đón khách", Color.yellow);
            }
            else
            {
                SystemNotify.instance.SendBigNoti("Bạn cần chở khách đến điểm này", Color.yellow);
            }
        }
    }
}
