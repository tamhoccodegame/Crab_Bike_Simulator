using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OppositeWay : MonoBehaviour
{
    public CarAI carAI;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carAI.SetTrafficSystemActive(false);

            StartCoroutine(OppositeWayCoroutine());
        }
       
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carAI.SetTrafficSystemActive(true);
        }
    }
    IEnumerator OppositeWayCoroutine() 
    {
        yield return new WaitForSeconds(3f);
    }
}
