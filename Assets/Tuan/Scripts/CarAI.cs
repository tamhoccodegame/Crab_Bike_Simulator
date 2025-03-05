using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public GameObject waypointsContainer;
    private TrafficLight trafficLight;
    public float speed = 10f;
    public float waypointThreshold = 1f;
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
        if (trafficLight.currentLightState == TrafficLight.LightState.Red)
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
            }

            MoveToNextWaypoint();
        }

    }


    public void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
           return;

        if (!agent.pathPending && agent.remainingDistance < waypointThreshold)
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
    }
    IEnumerator delayTraffic()
    {
        isTrafficSystemActive = false;
        yield return new WaitForSeconds(7f);
        Debug.Log("Wait for return traffic");
        isTrafficSystemActive = true;
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


