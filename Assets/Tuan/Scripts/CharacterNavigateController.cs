using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterNavigateController : MonoBehaviour
{
    public float movementSpeed;
    public float gravityStrength = -9.81f;
    public bool applyGravity = true;
    public float rotationSpeed = 700f;  
    public float stopDistance = 0.5f;    
    public Vector3 destination;         
    public bool reachedDestination = false;
    private Transform foot;
    private CharacterController controller;
    Vector3 moveDirection;

    [Header("NPC_Behaviour Controller")]
    public float delayTime;
    private bool isDelay;
    public MonoBehaviour[] npcBehaviourScripts;
    public Animator animator;


    private void OnEnable()
    {
        isDelay = false; 
        DisableBehaviour();
    }

    // Start is called before the first frame update
    void Start()
    {
        foot = transform.Find("Foot");
        controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
    {
       
    }
    void SetRandomMovementSpeed()
    {
        movementSpeed = Random.Range(1f, 3f);
    }
    void Update()
    {
        if (isDelay) return;
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            
            float destinationDistance = destinationDirection.magnitude;
            //Debug.Log(destinationDistance);
            if(destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation( destinationDirection );
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                moveDirection = destinationDirection.normalized * movementSpeed * Time.deltaTime;
            }
            else
            {
                StartCoroutine(DelaySetWayPoint());
            }
        }

        if (!Physics.Raycast(foot.position, transform.TransformDirection(Vector3.down), 0.1f))
        {
            moveDirection.y += gravityStrength * Time.deltaTime;
        }
        controller.Move(moveDirection);

    }

    IEnumerator DelaySetWayPoint()
    {
        isDelay = true;
        EnableBehaviour();
        GetComponent<CustomerBookCrab>().BookCrab();
        yield return new WaitForSeconds(10f);
        reachedDestination = true;
        isDelay = false;
        DisableBehaviour();
    }

    void DisableBehaviour()
    {
        Debug.Log("Disable on: " + gameObject.name);
        animator.Play("Walking");
        foreach (var m in npcBehaviourScripts)
        {
            if (m.enabled)
            {
                m.enabled = false;
            }
        }
    }

    void EnableBehaviour()
    {
        animator.Play("Idle");
        foreach (var m in npcBehaviourScripts)
        {
            if (!m.enabled)
            {
                m.enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
        SetRandomMovementSpeed();
    }
}
