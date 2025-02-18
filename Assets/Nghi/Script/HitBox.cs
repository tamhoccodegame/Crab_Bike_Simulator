using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum TargetType { Player, NPC }  // Xác định hitbox này đánh ai
    public TargetType targetType;  // Chọn mục tiêu trong Inspector

    public Collider hitboxCollider;  // Collider dùng làm hitbox
    public int damage = 10;  // Sát thương gây ra
    public float activeTime = 0.2f;  // Thời gian hitbox hoạt động
    public List<Collider> listDamage = new List<Collider>();  // Danh sách mục tiêu đã bị đánh

    private void Start()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (listDamage.Contains(other)) return; // Nếu đã đánh mục tiêu này rồi thì bỏ qua

        // Kiểm tra xem hitbox này đánh ai (Player hoặc NPC)
        if (targetType == TargetType.Player && other.CompareTag("Player"))
        {
            Player_Health playerHealth = other.GetComponent<Player_Health>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                //PlayerState playerState = other.GetComponent<PlayerState>();
                //if (playerState != null)
                //{
                //    playerState.DecreaseHealth(damage);
                //    listDamage.Add(other);
            }
        }
        else if (targetType == TargetType.NPC && other.CompareTag("NPC"))
        {
            NPC_Health npcHealth = other.GetComponent<NPC_Health>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(damage);
                listDamage.Add(other);
            }
        }
    }

    public void ActivateHitbox()
    {
        listDamage.Clear();
        hitboxCollider.enabled = true;
        //Invoke(nameof(DeactivateHitbox), activeTime);
    }

    public void DeactivateHitbox()
    {
        listDamage.Clear();
        hitboxCollider.enabled = false;
    }
}
