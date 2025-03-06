using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject waterEffect;
    private bool hasSpawnedWater = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 0.1f && !hasSpawnedWater)
        {
            var go = Instantiate(waterEffect, transform.position, Quaternion.identity);
            go.transform.rotation = Quaternion.Euler(-90, 0, 0);
            hasSpawnedWater = true;
            Destroy(gameObject, 5f);
        }
    }
}
