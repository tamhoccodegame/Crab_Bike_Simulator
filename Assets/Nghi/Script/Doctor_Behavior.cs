using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor_Behavior : MonoBehaviour, IInteractable
{
    public PlayerState playerState;
    public float healingCostPerHP = 5000; // Chi phí hồi mỗi 10 HP
    public float perHP = 10;
    public BubbleChat bubbleChatInstance;

    public KeyCode keyToInteract => KeyCode.E;

    private PlayerInteractor currentPlayer;

    public GameObject prompt;

    // Start is called before the first frame update
    void Start()
    {
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(prompt != null && prompt.activeSelf)
        {
            Vector3 direction = Camera.main.transform.position - prompt.transform.position;
            direction.y = 0; // Giữ nguyên trục Y để không bị nghiêng
            prompt.transform.rotation = Quaternion.LookRotation(direction);
        }

        if(currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            HealPlayer();
        }
    }


    public void HealPlayer()
    {

        if (playerState.currentHealth >= playerState.maxHealth)
        {
            bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Player's HP is fulled.");
            Debug.Log("Máu đã đầy.");
            return;
        }

        float missingHealth = playerState.maxHealth - playerState.currentHealth;
        float maxHealableHP = (PlayerCash.instance.currentCash / healingCostPerHP) * 10; // Tính số máu tối đa có thể hồi theo tiền

        if (maxHealableHP <= 0)
        {
            bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Player don't have enough minimum money to heal.");
            Debug.Log("Không đủ tiền để hồi máu.");
            return;
        }

        float healAmount = Mathf.Min(missingHealth, maxHealableHP);
        float cost = (healAmount / 10) * healingCostPerHP;

        // playerState.currentHealth += healAmount;
        PlayerCash.instance.CostMoney((int)cost);

        //// Gọi sự kiện cập nhật UI ngay sau khi hồi máu
        playerState.AddHealth(healAmount); //Gọi AddHealth() để hồi máu và cập nhật UI
        //playerState.OnHealthChange?.Invoke(playerState.currentHealth);

        bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Healed player!");
        //Debug.Log($"Đã hồi {healAmount} máu với giá {cost} VND. Máu hiện tại: {playerState.currentHealth}, Tiền còn lại: {playerState.money}");
    }

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
        prompt.SetActive(true);
    }

    public void ResetInteractState()
    {
        currentPlayer = null;
        prompt.SetActive(false);
    }
}
