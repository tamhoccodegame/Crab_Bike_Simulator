using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behavior : MonoBehaviour
{
   
    public enum NPCType { Friendly, Aggressive } // NPC hiền hoặc dữ
    public NPCType npcType; // Chọn loại NPC

    public Animator animator;
    public int attackAnimationCount;
    //public bool isAggressive;
    private bool isChasing = false;
    private bool isAttacking;

    public Transform policeStationTransform;
    public Transform playerTransform;
    private NavMeshAgent npcAgent;

    //public float npcAttackDamage;
    public float npcAttackRange;
    public float npcAttackCooldown;
    private float lastAttackTime;

    private bool canAttack = true;

    //public Animation_Random animation_Random;
    public NPC_Health npc_Health;
    public NPCInteractable npcInteractable;

    public HitBox hitBox;

    public Police_Behavior police_Behavior;
    //public bool isReportToPolice = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npcAgent = GetComponent<NavMeshAgent>();
        npc_Health = GetComponent<NPC_Health>();
        npcInteractable = GetComponent<NPCInteractable>();

        
        //if (animation_Random == null)
        //{
        //    animation_Random = GetComponent<Animation_Random>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            //Debug.Log($"{gameObject.name} đang tiếp tục truy đuổi/tấn công...");
            ApproachAndAttack();
        }
    }



    public void AttackOrReport()
    {
        //animation_Random.enabled = false;

        Debug.Log($"NPC {gameObject.name} đang quyết định hành động sau khi bị đánh.");
        if (npcType == NPCType.Aggressive)
        {
            
            npcInteractable.AggressiveOnHitByPlayer();
            Debug.Log($"{gameObject.name} chuyển sang tấn công Player!");
            //animation_Random.enabled = false;
            //NPC dữ sẽ truy đuổi để tấn công player
            isChasing = true;
            Debug.Log("Switching to Attack Animation");

            ApproachAndAttack();
        }
        else if (npcType == NPCType.Friendly)
        {
           
            npcInteractable.FriendlyOnHitByPlayer();
            Debug.Log($"{gameObject.name} hoảng sợ và chạy đến đồn cảnh sát.");
            //animation_Random.enabled = false;
            // NPC hiền chạy đến đồn cảnh sát
            StartCoroutine(ScaredAndReport(3f));

        }
    }

    IEnumerator ScaredAndReport(float time)
    {
        animator.SetTrigger("isScared");
        Debug.Log("Scared");
        yield return new WaitForSeconds(time);
        //animator.ResetTrigger("isScared");

        animator.SetBool("isRun", true);
        Debug.Log("Run");
        npcAgent.SetDestination(policeStationTransform.position);
        Debug.Log($"{gameObject.name} đang chạy đến đồn cảnh sát...");
        //Chờ NPC chạy tới đồn cảnh sát
        //while (npcAgent.remainingDistance > npcAgent.stoppingDistance)
        //{
        //    yield return null;
        //}

        while (npcAgent.pathPending || npcAgent.remainingDistance > npcAgent.stoppingDistance)
        {
            yield return null;
        }
        //Khi tới nơi, ngừng chạy và giữ animation "hoảng sợ"
        animator.SetBool("isRun", false);
        Debug.Log($"{gameObject.name} đã chạy đến đồn cảnh sát...");
        animator.ResetTrigger("isScared");
        animator.SetTrigger("isScared");
        //()()()()()()()()
        //Police_Behavior police_Behavior = GetComponent<Police_Behavior>();
        police_Behavior.ReceiveNPCReport(this);

        //isReportToPolice = true;

        //^^^^^^^^^^^^^^^^^^^^
        // Gọi tất cả cảnh sát cùng đuổi Player
        Police_Manager.Instance.AlertAllPolice(this);
    }


    private void ApproachAndAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        //Debug.Log($"Distance to Player: {distanceToPlayer} | Attack Range: {npcAttackRange}");

        if (npc_Health.isAttacked)// Nếu NPC đã bị tấn công, chỉ tập trung vào attack player
        {
            if (distanceToPlayer <= npcAttackRange)
            {
                // Đứng yên để tấn công
                npcAgent.isStopped = true;
                npcAgent.updatePosition = false;
                npcAgent.updateRotation = false;
                animator.SetBool("isApproach", false);
                //Debug.Log($"{gameObject.name} đang trong tầm tấn công!");

                //AttackPlayer();
                StartCoroutine(AttackPlayer());
            }
            else
            {
                // Tiếp cận player
                //Debug.Log($"{gameObject.name} đang tiến gần đến Player...");
                //Debug.Log("Approaching Player");
                npcAgent.isStopped = false;
                npcAgent.updatePosition = true;
                npcAgent.updateRotation = true;
                animator.SetBool("isApproach", true);
                npcAgent.SetDestination(playerTransform.position);
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        float timeSinceLastAttack = Time.time - lastAttackTime;
        //Debug.Log($"Checking attack cooldown: {timeSinceLastAttack} >= {npcAttackCooldown}");

        if (timeSinceLastAttack >= npcAttackCooldown)
        {
            canAttack = false;
            int randomAnimation = Random.Range(0, attackAnimationCount);
            animator.SetInteger("AttackIndex", randomAnimation);
            animator.SetTrigger("isAttack");

            Debug.Log($"{gameObject.name} Attacking with animation index: {randomAnimation}");

            lastAttackTime = Time.time;  // Cập nhật thời gian tấn công
            yield return null; // Chờ một frame để animation cập nhật

            // Đợi animation attack chạy xong trước khi tiếp tục^^^^^^^^^^^^^^
            //float attackAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
            //yield return new WaitForSeconds(attackAnimationTime+2f); // Chờ animation + cooldown
            canAttack = true;
            //Debug.Log($"{gameObject.name} đã hoàn thành đòn tấn công, chuẩn bị đòn tiếp theo...");
        }
        else
        {
            //Debug.Log($"{gameObject.name} chưa thể tấn công, cần chờ cooldown.");
        }





        //float timeSinceLastAttack = Time.time - lastAttackTime;
        //Debug.Log($"Checking attack cooldown: {timeSinceLastAttack} >= {npcAttackCooldown}");

        //if (timeSinceLastAttack >= npcAttackCooldown)  // Kiểm tra cooldown
        //{

        //    int randomAnimation = Random.Range(0, attackAnimationCount);
        //    animator.SetInteger("AttackIndex", randomAnimation);
        //    animator.SetTrigger("isAttack");
        //    Debug.Log($"NPC Attacking with animation index: {randomAnimation}");
        //    //*****************************************************************
        //    if (playerTransform != null)
        //    {
        //        Debug.Log($"[Damage] {gameObject.name} gây {npcAttackDamage} sát thương lên Player.");
        //        //Invoke(nameof(ActivateHitbox), 0.3f); // Bật hitbox khi đòn đánh sắp trúng
        //        //********************
        //        //playerTransform.GetComponent<Player_Health>().TakeDamage(npcAttackDamage);
        //    }
        //    else
        //    {
        //        Debug.LogError("LỖI: playerTransform bị NULL!");
        //    }

        //    lastAttackTime = Time.time;  // Reset thời gian tấn công
        //}
        //else
        //{
        //    //Debug.Log($"{gameObject.name} chưa thể tấn công, cần chờ cooldown.");
        //}
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
