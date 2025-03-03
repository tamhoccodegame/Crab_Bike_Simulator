using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public Animator animator;
    public PlayerState playerState;


    //^^^^^^^^^^^^^^^
    //public float healPrice;
    //public float healAmount;
    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = maxHeath;
        animator = GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (playerState == null)
        {
            Debug.LogError("PlayerState is missing on " + gameObject.name);
            
        }

        animator.SetTrigger("isHurt");

        playerState.DecreaseHealth(damage); // Gọi giảm máu ngay lập tức

    }



    private void Die()
    {
        Debug.Log("Player is Dead!");
        animator.enabled = false;
        GetComponent<Collider>().enabled = false;

        Ragdoll ragdoll = GetComponent<Ragdoll>();
        if (ragdoll != null)
        {
            ragdoll.EnableRagdoll();
        }
        else
        {
            Debug.LogError("Ragdoll component is missing on " + gameObject.name);
        }

    }
}

