using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    private float currentHealth;
    public float maxHeath;
    public Animator animator;
    public PlayerState playerState;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHeath;
        animator = animator.GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        //currentHealth -= damage;
        animator.SetTrigger("isHurt");
        //PlayerState playerState = GetComponent<PlayerState>();
        playerState.DecreaseHealth(damage);

        //NPC_Health npc_Health = npc.GetComponent<NPC_Health>();
        //npc_Health.TakeDamage(attackDamage);

        //if (currentHealth <= 0)
        //{
        //    Die();
        //}
    }

    //private void Die()
    //{
    //    Debug.Log("Player is Dead!");
    //    animator.enabled = false;
    //    GetComponent<Collider>().enabled = false;

    //    Ragdoll ragdoll = GetComponent<Ragdoll>();
    //    if (ragdoll != null)
    //    {
    //        ragdoll.EnableRagdoll();
    //    }
    //    else
    //    {
    //        Debug.LogError("Ragdoll component is missing on " + gameObject.name);
    //    }

    //}
}
