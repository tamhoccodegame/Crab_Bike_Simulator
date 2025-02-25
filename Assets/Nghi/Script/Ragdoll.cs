using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Animator animator;

    private enum RagdollState
    {
        Walking,
        Ragdoll
    }

    private RagdollState currentState = RagdollState.Walking;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        
        DisableRagdoll();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case RagdollState.Walking:
                WalkingBehavior();
                break;
            case RagdollState.Ragdoll:
                RagdollBehavior(); 
                break;
        }
    }

    public void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        animator.enabled = false;
    }

    private void WalkingBehavior()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnableRagdoll();
            currentState = RagdollState.Ragdoll;
        }
    }

    private void RagdollBehavior()
    {

    }
}
