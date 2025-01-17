using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public Transform[] waypoints;
    public TrafficLight trafficLight;
    public float speed = 10f;
    private int currentWaypointIndex = 0;

    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        MoveToNextWaypoint();
    }

    void Update()
    {
       

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
        if (trafficLight.currentLightState == TrafficLight.LightState.Green)
        {

            return;
        }
        else
        {

            MoveToNextWaypoint();
        }

    }

   
    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}


