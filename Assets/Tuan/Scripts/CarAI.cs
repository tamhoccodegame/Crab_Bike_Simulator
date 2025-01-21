using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public GameObject waypointsContainer;
    public TrafficLight trafficLight;
    public float speed = 10f;
    private int currentWaypointIndex = 0;
    private bool TrafficZone = false;
    private bool isTrafficSystemActive = true;
    private NavMeshAgent agent;
    private Transform[] waypoints;




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        if (waypointsContainer != null)
        {
            Transform[] allWaypoints = waypointsContainer.GetComponentsInChildren<Transform>();

            List<Transform> childWaypoints = new List<Transform>();

            foreach (Transform waypoint in allWaypoints)
            {
                if (waypoint != waypointsContainer.transform)
                {
                    childWaypoints.Add(waypoint);
                }
            }
            waypoints = childWaypoints.ToArray();
        }
    }
        

    void Update()
    {

        if (TrafficZone && isTrafficSystemActive)
        {
            
            TrafficSystem();
        }
        else
        {
            
            MoveToNextWaypointIfNeeded();
        }

    }
    public void SetTrafficSystemActive(bool isActive)
    {
        isTrafficSystemActive = isActive;
    }

    public void TrafficSystem()
    {
       
        if (trafficLight != null && trafficLight.currentLightState == TrafficLight.LightState.Red)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            MoveToNextWaypointIfNeeded();
        }
    }
    
    public void MoveToNextWaypointIfNeeded()
    {
       
        if (!agent.isStopped && !agent.pathPending && agent.remainingDistance < 0.5f)
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrafficLight"))
        {
            
            TrafficZone = true;
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TrafficLight"))
        {
            
            TrafficZone = false;
        }

    }


    

   
}


