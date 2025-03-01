using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Police_Behavior : MonoBehaviour
{
    public int attackAnimationCount;
    public HitBox hitBox;

    private Animator animator;
    private NavMeshAgent agent;

    public Transform playerTransform;

    private bool isChasing = false;
    private bool isAttacking = false;
    private bool canAttack = false;

    private float cooldownTimer = 0f;
    public float attackCooldown = 1.0f;
    public float attackRange = 1f;

    private NPCInteractable npcInteractable;

    private Police_Health police_Health;
    // Start is called before the first frame update
    void Start()
    {
        police_Health = GetComponent<Police_Health>();
        npcInteractable = GetComponent<NPCInteractable>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetCombatLayer(false);

        //^^^^^^^^^^^^^^^^^
        // Đăng ký cảnh sát này vào PoliceManager
        Police_Manager.Instance.RegisterPolice(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;// Giảm cooldown theo thời gian
        }

        if (isChasing)
        {
            //^%&^%^*&^*&)&(*)&(*&(*&(&()_*)*(*)_(*()*(_*)(_
            Approach();
        }
    }

    public void ReceiveNPCReport(NPC_Behavior reporter)
    {
        isChasing = true;
        Approach();
        npcInteractable.AggressiveOnHitByPlayer();
    }

    public void Approach()
    {
        isChasing = true;
        npcInteractable.AggressiveOnHitByPlayer();


        SetCombatLayer(true);
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        //%%%%%%%%%%%%%%%%%%%
        //if (police_Health.isAttacked == true)
        //{
            if (distanceToPlayer > attackRange)//Neu khoang cach lon hon tam danh thi tiep can gan
            {
                agent.isStopped = false;
                animator.SetBool("isApproach", true);
                agent.SetDestination(playerTransform.position);
                isChasing = true;
            }
            else if (distanceToPlayer <= attackRange && cooldownTimer <= 0)//Neu khoang cach du gan de o trong tam danh va thoi gian nghi giua don hop le,
                                                                           //dung tiep can va tan cong
            {
                agent.isStopped = true;
                animator.SetBool("isApproach", false);
                AttackPlayer();
            }
        //}

        
    }

    private void AttackPlayer()
    {
        //^^^^^^^^^^^^^^^^^^^^
        // Quay dần về hướng Player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);

        //canAttack = false;
        int randomeAttackAnimation = Random.Range(0, attackAnimationCount);
        animator.SetInteger("AttackIndex", randomeAttackAnimation);
        animator.SetTrigger("isAttack");

        //Attack xong thi reset timer
        cooldownTimer = attackCooldown;

        canAttack = true;
    }

    public void SetCombatLayer(bool active)
    {
        if (active)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetLayerWeight(0, 0);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    void ActivateHitbox()
    {
        hitBox.ActivateHitbox();
    }

    void DeactivateHitbox()
    {
        hitBox.DeactivateHitbox();
    }

}
