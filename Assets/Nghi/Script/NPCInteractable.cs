using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    //Start Adding 
    public BubbleChat bubbleChatInstance;
    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    [SerializeField] private string interactText;

    //public enum NPCType
    //{
    //    Business,
    //    Police,
    //    Dancer,
    //    Mafia,
    //    Doctor
    //}

    //[SerializeField] private NPCType npcType; // Chọn loại NPC

    [System.Serializable]
    public class DialogueSource
    {
        public Sprite icon;  // Icon biểu cảm
        public string text;  // Nội dung thoại
    }

    public enum NPCState
    {
        Normal,
        Angry,
        Scared
    }

    private NPCState currentState = NPCState.Normal;

    // Chuyển danh sách thoại thành Dictionary để dễ dàng gọi theo trạng thái
    private Dictionary<NPCState, List<DialogueSource>> dialogueDictionary = new Dictionary<NPCState, List<DialogueSource>>();

    [SerializeField] private List<DialogueSource> normalDialogues;
    [SerializeField] private List<DialogueSource> angryDialogues;
    [SerializeField] private List<DialogueSource> scaredDialogues;

    //[SerializeField] private List<DialogueSource> dialogueList = new List<DialogueSource>(); // Danh sách thoại chứa cả thoại & icon
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private Transform playerTransform;

    private Transform currentDialogueBoxChatTransform; // Lưu box chat hiện tại (Dialog BoxChat)
    private bool isShowingBoxChat = false;
    private float lastChatTime = -10f;
    //***************************
    private bool isInteracting; // Kiểm tra có đang tương tác không
    private bool canShowDialogueBoxChat = true; // Điều khiển việc hiển thị Dialogue Box Chat
    private bool isPlayerInRange = false; // Kiểm tra player có đang trong detectRange không
    //*************
    private float detectBuffer = 0.3f; // Thêm một khoảng buffer để tránh chớp nháy




    public float healPrice;
    public float healAmount;

    [Header("Tam IInteractable")]
    public GameObject prompt;
    private PlayerInteractor currentPlayer;
    public KeyCode keyToInteract => KeyCode.E;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();

        // Gán các danh sách thoại vào Dictionary
        dialogueDictionary[NPCState.Normal] = normalDialogues;
        dialogueDictionary[NPCState.Angry] = angryDialogues;
        dialogueDictionary[NPCState.Scared] = scaredDialogues;

        if(prompt != null)
        {
            prompt.SetActive(false);
        }
    }

    private void Update()
    {
        //FindPlayerAndDecide();

        if(currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            Interact(currentPlayer.transform);
        }

        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Khi player ở trong detectRange
        if (distance <= detectRange)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;

                // Khi player vừa quay lại, cho phép hiển thị lại Dialogue Box Chat
                if (!isInteracting)
                {
                    canShowDialogueBoxChat = true;
                    ShowRandomDialog();
                    Debug.Log("Show from Update!");
                }
            }
        }
        //****************************
        else if (distance > detectRange + detectBuffer) // Chỉ tắt khi Player đi ra xa hơn một chút
        {
            // Khi player rời khỏi detectRange
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                isShowingBoxChat = false;
                canShowDialogueBoxChat = false; // Ngăn chặn hiển thị hộp thoại khi player đang đi ra ngoài
                //***
                HideDialogBoxChat(14f);
            }
        }
    }

    public void Interact(Transform interactorTransform)
    {
        if (isInteracting) return; // Nếu đang tương tác, không làm gì cả

        isInteracting = true; // Đánh dấu đang tương tác
        canShowDialogueBoxChat = false; // Ngăn không cho Dialogue Box Chat xuất hiện khi Interact Box Chat đang hiển thị


        // Xóa Dialogue Box Chat ngay lập tức nếu nó đang hiển thị
        ClearDialogueBox();

        if (gameObject.layer == LayerMask.NameToLayer("Doctor"))
        {
            if (interactorTransform == null)
            {
                Debug.LogWarning("InteractorTransform is NULL!");
                return;
            }

            Debug.Log($"{gameObject.name} được tương tác bởi {interactorTransform.name}");

            
            Doctor_Behavior doctor_Behavior = GetComponent<Doctor_Behavior>();
            doctor_Behavior.HealPlayer();
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            


        }
        else
        {
            bubbleChatInstance.Create(transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Chào bạn nhé! Một ngày tốt lành");
        }
        
        //bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Hello there! Nice to meet you!");

        animator.SetTrigger("isWaving");

        float playerHeight = 1.7f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);

        //StartCoroutine(EndInteractBoxChat(3f, bubbleChatInstance));
        // Ẩn sau 3 giây và cho phép Dialogue Box Chat hoạt động lại
        StartCoroutine(EndInteraction(3f));
    }

    // Kết thúc tương tác, cho phép Dialogue Box Chat xuất hiện trở lại
    IEnumerator EndInteraction(float time)
    {
        yield return new WaitForSeconds(time);

        ClearDialogueBox();

        isInteracting = false; // Cho phép Dialogue Box Chat hiển thị trở lại
        
        yield return new WaitForSeconds(1f); // Chờ 1 giây trước khi cho phép Dialogue Box Chat hiển thị lại
        canShowDialogueBoxChat = true;
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void ShowRandomDialog()
    {
        if (isShowingBoxChat) return; // Nếu đang hiển thị, không tạo hộp thoại mới
        //if (dialogueList.Count == 0||isInteracting||!canShowDialogueBoxChat) return; // Không hiển thị nếu đang tương tác
        if (!dialogueDictionary.ContainsKey(currentState) || dialogueDictionary[currentState].Count == 0 || isInteracting || !canShowDialogueBoxChat) return;


        // Xóa hộp thoại cũ nếu còn tồn tại
        ClearDialogueBox();

        //DialogueSource randomDialog = dialogueList[Random.Range(0, dialogueList.Count)];
        List<DialogueSource> dialogues = dialogueDictionary[currentState];
        DialogueSource randomDialog = dialogues[Random.Range(0, dialogues.Count)];

        if (bubbleChatInstance != null)
        {
            currentDialogueBoxChatTransform = bubbleChatInstance.CreateDialogue(transform, new Vector3(0.8f, 2.3f, 0f), randomDialog.icon, randomDialog.text);

            isShowingBoxChat = true;
            lastChatTime = Time.deltaTime;

            StartCoroutine(HideDialogBoxChat(14f));
        }
    }

    IEnumerator HideDialogBoxChat(float time)
    {
        //***
        isShowingBoxChat = false; // Đánh dấu trước khi chờ

        yield return new WaitForSeconds(time);

        ClearDialogueBox();

        // Đợi 2 giây, nếu player vẫn trong phạm vi thì hiển thị lại box chat mới
        yield return new WaitForSeconds(2f);
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectRange)
        {
            ShowRandomDialog();
            Debug.Log("Show after 2 secs if player still in Range from HideDialogBoxChat!");
        }
    }

    private void ClearDialogueBox()
    {
        if (currentDialogueBoxChatTransform != null)
        {
            Destroy(currentDialogueBoxChatTransform.gameObject);
            currentDialogueBoxChatTransform = null;
        }
        isShowingBoxChat = false;
    }

    // **Khi NPC bị tấn công**
    public void AggressiveOnHitByPlayer()
    {
        if (currentState != NPCState.Angry) // Nếu chưa tức giận thì mới đổi trạng thái
        {
            currentState = NPCState.Angry;
            ShowRandomDialog();
            Debug.Log("Show from AggresiveOnHitByPlayer!");
        }
    }

    public void FriendlyOnHitByPlayer()
    {
        if (currentState != NPCState.Scared)
        {
            currentState = NPCState.Scared;
            ShowRandomDialog();
            Debug.Log("Show from FriendlyOnHitByPlayer!");
        }
    }

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
        isInteracting = false;
        prompt.SetActive(true);
        
    }

    public void ResetInteractState()
    {
        currentPlayer = null;
        prompt.SetActive(false);
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        ClearDialogueBox();
    }
}
