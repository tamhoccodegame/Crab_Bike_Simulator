using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Health : MonoBehaviour
{
    private float npcMaxHeath = 100f;
    private float npcCurrentHeath;
    public Animator animator;
    public NPC_Behavior npc_Behavior;
    public bool isAggressive;

    private bool isDead = false;
    public bool isAttacked = false;
    ///public Animation_Random animation_Random;

    [SerializeField] private NPC_Health_UI npcHealth_UI;

    //private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetCombatLayerActive(false); // Mặc định tắt Combat Layer

        npcCurrentHeath = npcMaxHeath;
        //Debug.Log($"{gameObject.name} khởi tạo với máu: {npcCurrentHeath}");

        animator = GetComponent<Animator>();

        ////*************************************************
        // Tìm HealthBar trong hệ thống UI thay vì trong NPC
        //GameObject healthBarPrefab = UI_Manager.Instance.GetHealthBar();
        //if (healthBarPrefab == null)
        //{
        //    Debug.LogError("Không tìm thấy HealthBar Prefab trong UI_Manager!");
        //    return;
        //}

        //GameObject healthBarObj = Instantiate(healthBarPrefab, UI_Manager.Instance.canvas.transform);
        //npcHealth_UI = healthBarObj.GetComponent<NPC_Health_UI>();

        //if (npcHealth_UI != null)
        //{
        //    npcHealth_UI.Initialize(transform);
        //}

        //if (npcHealth_UI == null)
        //{
        //    Debug.LogError("HealthBarObj không có NPC_Health_UI!");
        //    return;
        //}



    }

    // Update is called once per frame
    void Update()
    {
        //npcHealth_UI.UpdateHeathUI(npcCurrentHeath, npcMaxHeath);
    }

    public void SetCombatLayerActive(bool active)
    {
        if (active)
        {
            animator.SetLayerWeight(1, 1); // Bật Combat Layer

            animator.SetLayerWeight(0, 0);//Tat Normal Layer
        }
        else
        {
            animator.SetLayerWeight(1, 0); // Tắt Combat Layer, chỉ chạy Base Layer
        }
    }

    public void TakeDamage(float damage)
    {
        npc_Behavior.enabled = true;
        npcCurrentHeath -= damage;
        animator.SetTrigger("isHurt");

        SetCombatLayerActive(true);
        //@@@@@@@@@@@@@@@@@@@@@@@
        if (npcHealth_UI != null)
        {
            npcHealth_UI.UpdateHeathUI(npcCurrentHeath, npcMaxHeath);
        }
        Debug.Log(npcCurrentHeath);
        //Debug.Log($"{gameObject.name} bị tấn công! Máu trước khi trừ: {npcCurrentHeath}, Sát thương nhận: {damage}");
        //Debug.Log($"{gameObject.name} sau khi bị tấn công! Máu còn lại: {npcCurrentHeath}");

        if (isDead)
        {
            Debug.Log("Lỗi: NPC đã chết từ trước nhưng vẫn nhận sát thương!");
            return; // Không trừ máu nữa
        }

        isAttacked = true; // Đánh dấu NPC đang bị tấn công

        //**********%%%%%%%%%%%%%%%%%
        StartCoroutine(ReactAfterHurt());

    }

    IEnumerator ReactAfterHurt()
    {
        if (npcCurrentHeath <= 0)
        {
            Die();
        }
        // Chờ animation "isHurt" chạy xong
        yield return new WaitForSeconds(1f);

        animator.SetTrigger("isBackToIdle");

        
        {
            //StartCoroutine(ReactAfterHurt());
            npc_Behavior.AttackOrReport();
        }

        //npc_Behavior.AttackOrReport();
    }

    public void Die()
    {
        if (isDead) return;

        GetComponent<AudioSource>().Play();

        if(TryGetComponent<CustomerBookCrab>(out CustomerBookCrab c) && c.debt > 0)
        {
            c.ResetDebt();
        }

        Debug.Log($"{this.name} is Dead!");
        isDead = true;

        animator.enabled = false;
        StartCoroutine(DisableCollider());
        GetComponent<NavMeshAgent>().enabled = false;

        foreach(var m in GetComponents<MonoBehaviour>())
        {
            if (m == this) continue;
            m.enabled = false;
        }
        //agent.enabled = false;

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

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }


}
