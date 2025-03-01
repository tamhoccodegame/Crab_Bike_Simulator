using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public GameObject waypointsContainer;
    private TrafficLight trafficLight;
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

        waypoints = GetWaypoints(waypointsContainer);
    }
        

    void Update()
    {

        if (TrafficZone && isTrafficSystemActive)
        {
            TrafficSystem();
        }
        else
        {
            MoveToNextWaypoint();
        }

    }
    public void SetTrafficSystemActive(bool isActive)
    {
        isTrafficSystemActive = isActive;
    }

    public void TrafficSystem()
    {
        if (trafficLight != null)
        {
            Debug.Log("Current Traffic Light State: " + trafficLight.currentLightState);

            if (trafficLight.currentLightState == TrafficLight.LightState.Red)
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
            }
            else
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                }

                MoveToNextWaypoint();
            }
        }
        else
        {
            MoveToNextWaypoint();
        }
    }


    public void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
           return;

        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (!agent.isStopped)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.destination = waypoints[currentWaypointIndex].position;
                Debug.Log("Moving to waypoint: " + currentWaypointIndex);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrafficLight"))
        {
            TrafficZone = true;
            trafficLight = other.GetComponent<TrafficLight>();
        }
        if (other.CompareTag("Reverse"))
        {
            StartCoroutine(delayTraffic());
            Debug.Log("Car is going in the opposite direction, ignoring traffic lights.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TrafficLight"))
        {
            TrafficZone = false;
            trafficLight = null;
            if (!isTrafficSystemActive)
            {
                MoveToNextWaypoint();
            }
        }
        if (other.CompareTag("Reverse"))
        {
            Debug.Log("Exited Reverse Zone.");
            StartCoroutine(ResumeTrafficSystem());
        }
    }
    IEnumerator delayTraffic()
    {
        TrafficZone = false;
        yield return new WaitForSeconds(1f);
        TrafficZone = true;
    }
    IEnumerator ResumeTrafficSystem()
    {
        yield return new WaitForSeconds(1f); // Optional: Delay before resuming
        TrafficZone = true;
        Debug.Log("Resumed traffic system.");
    }
    Transform[] GetWaypoints(GameObject container)
    {
        Transform[] allTransforms = container.GetComponentsInChildren<Transform>();

        List<Transform> waypointsList = new List<Transform>();

        foreach (Transform t in allTransforms)
        {
            if (t != container.transform)
            {
                waypointsList.Add(t);
            }
        }

        // Convert list to array and return
        return waypointsList.ToArray();
    }

}


