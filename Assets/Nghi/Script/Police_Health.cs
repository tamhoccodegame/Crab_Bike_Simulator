using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police_Health : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    private Animator animator;

    public bool isAttacked = false;
    private Police_Behavior policeBehavior;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        policeBehavior = GetComponent<Police_Behavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("isHurt");
        
        isAttacked = true;

        StartCoroutine(ReactAfterHurt());
    }

    IEnumerator ReactAfterHurt()
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("isBackToIdle");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //***********************
            policeBehavior.Approach();
        }
    }

    private void Die()
    {

    }
}
