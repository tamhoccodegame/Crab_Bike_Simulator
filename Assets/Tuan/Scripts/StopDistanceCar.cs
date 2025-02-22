using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopDistanceCar : MonoBehaviour
{
    private bool isCarInFront = false;
    private NavMeshAgent agent;
    private CarAI carAI;
    private bool isRearCar = false;

    void Start()
    {
        carAI = GetComponent<CarAI>();
        if (carAI != null)
        {
            agent = carAI.GetComponent<NavMeshAgent>();

            if (agent == null)
            {
                Debug.LogError("The NavMeshAgent component is missing on the CarAI.");
            }

            if (!agent.isOnNavMesh)
            {
                Debug.LogError("The NavMeshAgent is not on the NavMesh!");
            }
        }
        else
        {
            Debug.LogError("CarAI not found on the object.");
        }
        if (CompareTag("Car"))
        {
            isRearCar = false;
        }
    }

    void Update()
    {
        if (isRearCar)
        {
            if (isCarInFront)
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                    Debug.Log("Car stopped due to car in front.");
                }
            }
            else
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                    carAI.MoveToNextWaypoint();
                    Debug.Log("Car is moving to next waypoint.");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (!isRearCar && !isCarInFront)
            {
                isCarInFront = true;
                tag = "RearCar";
                isRearCar = true;
                Debug.Log("Car in front detected. Marked as rear car.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("Exit trigger with another car.");

            if (isRearCar)
            {
                tag = "Car";
                isCarInFront = false;
                isRearCar = false;
                agent.isStopped = false;
                carAI.MoveToNextWaypoint();
                Debug.Log("Rear car resumes moving to next waypoint.");
            }
        }
    }
}
