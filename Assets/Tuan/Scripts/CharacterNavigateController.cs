using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigateController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed = 700f;  
    public float stopDistance = 0.5f;    
    public Vector3 destination;         
    public bool reachedDestination = false; 
    private Vector3 velocity;            
    private Vector3 lastPosition;
    private Animator _animator;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        lastPosition = transform.position;
        SetRandomMovementSpeed();
    }
    void SetRandomMovementSpeed()
    {
        movementSpeed = Random.Range(1f, 3f);
    }
    void Update()
    {
        if(transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            
            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= stopDistance )
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation( destinationDirection );
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition)/Time.deltaTime;
            velocity.y = 0;
            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProduct = Vector3.Dot(transform.right, velocity);

            _animator.SetFloat("Horizontal", rightDotProduct);
            _animator.SetFloat("Forward", fwdDotProduct);
        }
        else
        {
            // Optionally, add logic for idle animation here when the destination is reached
            _animator.SetFloat("Forward", 0);
            _animator.SetFloat("Horizontal", 0);
        }
    }
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
        SetRandomMovementSpeed();
    }
}
