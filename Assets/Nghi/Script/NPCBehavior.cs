using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    public Collider damageCollider;  // Collider dùng làm hitbox
    public List<Collider> listDamage = new List<Collider>();  // Danh sách mục tiêu đã bị đánh
    public void ActivateHitbox()
    {
        listDamage.Clear();
        damageCollider.enabled = true;
        //Invoke(nameof(DeactivateHitbox), activeTime);
    }

    public void DeactivateHitbox()
    {
        listDamage.Clear();
        damageCollider.enabled = false;
    }

}
