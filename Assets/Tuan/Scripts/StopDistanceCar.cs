using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopDistanceCar : MonoBehaviour
{
    private bool CarZone = false;
    public NavMeshAgent agent;
    public CarAI carAI;


    void Start()
    {
        agent = carAI.GetComponent<NavMeshAgent>();
    }

    void Update()
    {


        if (CarZone == true)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            carAI.MoveToNextWaypointIfNeeded();
        }



    }


    void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Car"))
        {
            CarZone = true;
        }

    }


    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Car"))
        {
            CarZone = false;
        }

    }
}

