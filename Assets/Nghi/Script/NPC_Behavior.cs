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
    
    private bool isChasing = false;
    private bool isAttacking;

    public Transform policeStationTransform;
    public Transform playerTransform;
    private NavMeshAgent npcAgent;

    
    public float npcAttackRange;
    public float npcChaseRange;
    public float npcAttackCooldown;
    private float lastAttackTime;

    private bool canAttack = true;

    
    public NPC_Health npc_Health;
    public NPCInteractable npcInteractable;

    public HitBox hitBox;

    public Police_Behavior police_Behavior;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npcAgent = GetComponent<NavMeshAgent>();
        npc_Health = GetComponent<NPC_Health>();
        npcInteractable = GetComponent<NPCInteractable>();
        playerTransform = GameObject.FindWithTag("Player").transform.Find("PlayerVisual");
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            RotateSmoothlyTowards(playerTransform.position);
            ApproachAndAttack();
        }


    }

    void RotateSmoothlyTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Giữ NPC thẳng đứng

        if (direction.magnitude > 0.01f) // Kiểm tra nếu có hướng xoay hợp lệ
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 200f);
        }
    }
        
    public void AttackOrReport()
    {
        GetComponent<CharacterNavigateController>().enabled = false;
        //animation_Random.enabled = false;

        Debug.Log($"NPC {gameObject.name} đang quyết định hành động sau khi bị đánh.");
        if (npcType == NPCType.Aggressive)
        {
            
            npcInteractable.AggressiveOnHitByPlayer();
            Debug.Log($"{gameObject.name} chuyển sang tấn công Player!");
            
            //NPC dữ sẽ truy đuổi để tấn công player
            isChasing = true;
            Debug.Log("Switching to Attack Animation");

            ApproachAndAttack();
        }
        else if (npcType == NPCType.Friendly)
        {
           
            npcInteractable.FriendlyOnHitByPlayer();
            Debug.Log($"{gameObject.name} hoảng sợ và chạy đến đồn cảnh sát.");
            
            // NPC hiền chạy đến đồn cảnh sát
            StartCoroutine(ScaredAndReport(3f));

        }
    }

    IEnumerator ScaredAndReport(float time)
    {
        animator.SetTrigger("isScared");
        Debug.Log("Scared");
        yield return new WaitForSeconds(time);

        animator.SetBool("isRun", true);
        Debug.Log("Run");
        npcAgent.SetDestination(policeStationTransform.position);
        Debug.Log($"{gameObject.name} đang chạy đến đồn cảnh sát...");


        while (npcAgent.pathPending || npcAgent.remainingDistance > npcAgent.stoppingDistance)
        {
            yield return null;
        }
        //Khi tới nơi, ngừng chạy và giữ animation "hoảng sợ"
        animator.SetBool("isRun", false);
        Debug.Log($"{gameObject.name} đã chạy đến đồn cảnh sát...");
        animator.ResetTrigger("isScared");
        animator.SetTrigger("isScared");
        
        police_Behavior.ReceiveNPCReport(this);

       
        // Gọi tất cả cảnh sát cùng đuổi Player
        Police_Manager.Instance.AlertAllPolice(this);
    }


    private void ApproachAndAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //Nếu đang trong animation attack thì không tiếp cận
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        Debug.Log($"Distance to Player: {distanceToPlayer} | Attack Range: {npcAttackRange}");

        if (distanceToPlayer > npcChaseRange)
        {
            Debug.Log("Out of chase Range, enable CharacterNavigate");
            GetComponent<CharacterNavigateController>().enabled = true;
            isChasing = false;
            npc_Health.SetCombatLayerActive(false);
            return;
        }

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
                npcAgent.isStopped = false;
                npcAgent.updatePosition = true;
                //npcAgent.updateRotation = true;
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

            ////^^^^^^^^^^^^^^^^

            // Dừng di chuyển hoàn toàn khi đang attack
            npcAgent.isStopped = true;
            npcAgent.velocity = Vector3.zero; // Reset vận tốc để tránh trượt
            npcAgent.updatePosition = false;
            npcAgent.updateRotation = false;
            ////^^^^^^^^^^^^^^^^

            // Lưu vị trí NPC trước khi attack
            Vector3 fixedPosition = transform.position;
            // Xoay trước khi tấn công
            RotateSmoothlyTowards(playerTransform.position);

            animator.SetTrigger("isAttack");

            lastAttackTime = Time.time;  // Cập nhật thời gian tấn công

            // Đợi animation attack chạy xong trước khi tiếp tục
            //yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Đợi animation đánh xong

            // Lấy thời gian animation attack
            float attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;

            // Giữ nguyên vị trí NPC suốt thời gian attack
            float elapsedTime = 0f;
            while (elapsedTime < 1.8)
            {
                transform.position = fixedPosition; // Cố định vị trí
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //Chỉ sau khi attack kết thúc mới cho phép di chuyển tiếp
            npcAgent.isStopped = false;
            npcAgent.updatePosition = true;
            npcAgent.updateRotation = true;
            canAttack = true;
            //Debug.Log($"{gameObject.name} đã hoàn thành đòn tấn công, chuẩn bị đòn tiếp theo...");
        }
        else
        {
            //Debug.Log($"{gameObject.name} chưa thể tấn công, cần chờ cooldown.");
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
