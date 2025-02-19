    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;

    public class OppositeWay : MonoBehaviour
    {
        private CarAI carAI;
    void Start()
    {
        carAI = GetComponent<CarAI>();
        // If for some reason CarAI is not found, print an error
        if (carAI == null)
        {
            Debug.LogError("No CarAI found in the scene!");
        }
    }
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
            
            yield return new WaitForSeconds(2f);
        }
    }
