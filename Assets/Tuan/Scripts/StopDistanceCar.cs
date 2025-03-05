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
                    agent.velocity = Vector3.zero;
                }
            }
            else
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                    carAI.MoveToNextWaypoint();
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
            }
        }

        if (other.CompareTag("Player"))
        {
            if (!isRearCar && !isCarInFront)
            {
                isCarInFront = true;
                tag = "RearCar";
                isRearCar = true;
            }
        }
        if (other.CompareTag("NPC"))
        {
            if (!isRearCar && !isCarInFront)
            {
                isCarInFront = true;
                tag = "RearCar";
                isRearCar = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {

            if (isRearCar)
            {
                tag = "Car";
                isCarInFront = false;
                isRearCar = false;
                agent.isStopped = false;
                carAI.MoveToNextWaypoint();
            }
        }
        if (other.CompareTag("Player"))
        {
            if (isRearCar)
            {
                tag = "Car";
                isCarInFront = false;
                isRearCar = false;
                agent.isStopped = false;
                carAI.MoveToNextWaypoint();
            }
        }
        if (other.CompareTag("NPC"))
        {
            if (isRearCar)
            {
                tag = "Car";
                isCarInFront = false;
                isRearCar = false;
                agent.isStopped = false;
                carAI.MoveToNextWaypoint();
            }
        }
    }
}
