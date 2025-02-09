using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    public BubbleChat bubbleChatInstance;
    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    [SerializeField] private string interactText;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform)
    {
        bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Hello there! Nice to meet you!");

        animator.SetTrigger("isWaving");

        float playerHeight = 1.7f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
        
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
