using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopDistanceCar : MonoBehaviour
{
    private bool CarZone = false;
    private NavMeshAgent agent;
    private CarAI carAI;


    void Start()
    {
        carAI = GetComponent<CarAI>();
        if (carAI != null)
        {
            agent = carAI.GetComponent<NavMeshAgent>();

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

            if (carAI != null)
            {
                carAI.MoveToNextWaypoint();
            }
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

