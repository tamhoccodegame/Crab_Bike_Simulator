using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Health : MonoBehaviour
{
    private float npcMaxHeath = 100f;
    private float npcCurrentHeath;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        npcCurrentHeath = 100f;
        animator = animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (npcCurrentHeath > 0)
        {
            npcCurrentHeath -= damage;
            animator.SetTrigger("isHurt");
        }else if (npcCurrentHeath <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

    }
}
