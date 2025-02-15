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
        if (carAI != null)
        {
            agent = carAI.GetComponent<NavMeshAgent>();

            // Ensure the agent is placed on the NavMesh before doing anything
            if (!agent.isOnNavMesh)
            {
                Debug.LogError("The NavMeshAgent is not on the NavMesh!");
            }
        }
        else
        {
            Debug.LogError("CarAI not assigned in StopDistanceCar.");
        }
    }

    void Update()
    {


        if (CarZone)
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

