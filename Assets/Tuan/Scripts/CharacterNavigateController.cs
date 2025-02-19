using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigateController : MonoBehaviour
{
    public float movementSpeed;
    public float gravityStrength = -9.81f;
    public bool applyGravity = true;
    public float rotationSpeed = 700f;  
    public float stopDistance = 0.5f;    
    public Vector3 destination;         
    public bool reachedDestination = false;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRandomMovementSpeed();
        if (rb != null && applyGravity)
        {
            rb.useGravity = false;
        }
    }
    void FixedUpdate()
    {
        if (applyGravity && rb != null)
        {
            Vector3 gravityForce = new Vector3(0, gravityStrength, 0);
            rb.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }
    void SetRandomMovementSpeed()
    {
        movementSpeed = Random.Range(1f, 3f);
    }
    void Update()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            
            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= stopDistance )
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation( destinationDirection );
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                Vector3 moveDirection = destinationDirection.normalized * movementSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + moveDirection);
            }
            else
            {
                reachedDestination = true;
            }
        }
      
      
    }
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
        SetRandomMovementSpeed();
    }
}
