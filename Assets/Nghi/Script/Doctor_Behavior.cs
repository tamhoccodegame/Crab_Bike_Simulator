using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class Doctor_Behavior : MonoBehaviour
{
    public PlayerState playerState;
    public float healingCostPerHP = 5000; // Chi phí hồi mỗi 10 HP
    public float perHP = 10;
    public BubbleChat bubbleChatInstance;

    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void HealPlayer()
    {
        PlayerState playerState = FindObjectOfType<PlayerState>(); // Tìm PlayerStats

        if (playerState == null) return;

        if (playerState.currentHealth >= playerState.maxHealth)
        {
            bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Player's HP is fulled.");
            Debug.Log("Máu đã đầy.");
            return;
        }

        float missingHealth = playerState.maxHealth - playerState.currentHealth;
        float maxHealableHP = (playerState.money / healingCostPerHP) * 10; // Tính số máu tối đa có thể hồi theo tiền

        if (maxHealableHP <= 0)
        {
            bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Player don't have enough minimum money to heal.");
            Debug.Log("Không đủ tiền để hồi máu.");
            return;
        }

        float healAmount = Mathf.Min(missingHealth, maxHealableHP);
        float cost = (healAmount / 10) * healingCostPerHP;

        playerState.currentHealth += healAmount;
        playerState.money -= cost;
        bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Healed player!");
        Debug.Log($"Đã hồi {healAmount} máu với giá {cost} VND. Máu hiện tại: {playerState.currentHealth}, Tiền còn lại: {playerState.money}");
    }

}
