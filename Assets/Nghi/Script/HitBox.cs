using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum TargetType { Player, NPC }  // Xác định hitbox này đánh ai
    public TargetType targetType;  // Chọn mục tiêu trong Inspector

    public Collider hitboxCollider;  // Collider dùng làm hitbox
    public int damage;  // Sát thương gây ra
   
    //public List<Collider> listDamage = new List<Collider>();  // Danh sách mục tiêu đã bị đánh
    public List<Transform> listDamage = new List<Transform>();  // Danh sách mục tiêu đã bị đánh

    private void Start()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootTransform = other.transform.root; // Lấy root GameObject (cơ thể nhân vật)
        if (listDamage.Contains(rootTransform)) return; // Nếu đã trúng nhân vật này rồi thì bỏ qua
        //if (listDamage.Contains(other)) return; // Nếu đã đánh mục tiêu này rồi thì bỏ qua

        Debug.Log($"Hitbox {gameObject.name} va chạm với {other.name}"); // Kiểm tra số lần gọi

        // Kiểm tra xem hitbox này đánh ai (Player hoặc NPC)
        if (targetType == TargetType.Player && other.CompareTag("Player"))
        {
            Player_Health playerHealth = rootTransform.GetComponent<Player_Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"{gameObject.name} đã gây {damage} sát thương lên {other.name}"); // Kiểm tra số lần gây damage

                listDamage.Add(rootTransform);// Chỉ thêm root vào danh sách để tránh trùng lặp

            }
        }
        else if (targetType == TargetType.NPC && other.CompareTag("NPC"))
        {
            NPC_Health npcHealth = other.GetComponent<NPC_Health>();
            Police_Health police_Health = other.GetComponent<Police_Health>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(damage);
                Debug.Log($"{gameObject.name} đã va chạm với {other.name}");
                listDamage.Add(rootTransform);
            }

            if (police_Health != null)
            {
                police_Health.TakeDamage(damage);
                Debug.Log($"{gameObject.name} đã gây {damage} sát thương lên cảnh sát {other.name}");
                listDamage.Add(rootTransform);
            }
        }
    }

    public void ActivateHitbox()
    {
        listDamage.Clear();
        if (!hitboxCollider.enabled) // Đảm bảo hitbox chưa bật
        {
            hitboxCollider.enabled = true;
            Debug.Log($"Hitbox {gameObject.name} được kích hoạt");
        }
    }

    public void DeactivateHitbox()
    {

        if (hitboxCollider.enabled) // Đảm bảo chỉ tắt nếu đang bật
        {
            hitboxCollider.enabled = false;
            listDamage.Clear();
            Debug.Log($"Hitbox {gameObject.name} đã tắt");
        }
    }


}
