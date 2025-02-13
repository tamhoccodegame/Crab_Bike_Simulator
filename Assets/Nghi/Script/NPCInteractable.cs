using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
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

    [SerializeField] private List<DialogueSource> dialogueList = new List<DialogueSource>(); // Danh sách thoại chứa cả thoại & icon
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private Transform playerTransform;

    private Transform currentDialogueBoxChatTransform; // Lưu box chat hiện tại (Dialog BoxChat)
    private bool isShowingBoxChat = false;
    private float lastChatTime = -10f;
    //***************************
    private bool isInteracting; // Kiểm tra có đang tương tác không
    private bool canShowDialogueBoxChat = true; // Điều khiển việc hiển thị Dialogue Box Chat
    private bool isPlayerInRange = false; // Kiểm tra player có đang trong detectRange không

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    private void Update()
    {
        FindPlayerAndDecide();

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
                }
            }
        }
        else
        {
            // Khi player rời khỏi detectRange
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                isShowingBoxChat = false;
                canShowDialogueBoxChat = false; // Ngăn chặn hiển thị hộp thoại khi player đang đi ra ngoài
            }
        }
    }

    public void Interact(Transform interactorTransform)
    {
        if (isInteracting) return; // Nếu đang tương tác, không làm gì cả

        isInteracting = true; // Đánh dấu đang tương tác
        canShowDialogueBoxChat = false; // Ngăn không cho Dialogue Box Chat xuất hiện khi Interact Box Chat đang hiển thị


        // Xóa Dialogue Box Chat ngay lập tức nếu nó đang hiển thị
        if (currentDialogueBoxChatTransform != null)
        {
            Destroy(currentDialogueBoxChatTransform.gameObject);
            currentDialogueBoxChatTransform = null;
            isShowingBoxChat = false;
        }

        bubbleChatInstance.Create(transform.transform, new Vector3(0.8f, 2.3f, 0f), BubbleChat.IconType.Happy, "Hello there! Nice to meet you!");

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

        if (currentDialogueBoxChatTransform != null)
        {
            Destroy(currentDialogueBoxChatTransform.gameObject);
            currentDialogueBoxChatTransform = null;
        }

        isInteracting = false; // Cho phép Dialogue Box Chat hiển thị trở lại
        
        yield return new WaitForSeconds(1f); // Chờ 1 giây trước khi cho phép Dialogue Box Chat hiển thị lại
        canShowDialogueBoxChat = true;
    }

    public string GetInteractText()
    {
        return interactText;
    }

    private void FindPlayerAndDecide()
    {
        
        if (playerTransform == null || isInteracting ||!canShowDialogueBoxChat) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= detectRange && !isShowingBoxChat && Time.deltaTime - lastChatTime >= 2f)
        {
            ShowRandomDialog();
        }
    }

    public void ShowRandomDialog()
    {
        
        if (dialogueList.Count == 0||isInteracting||!canShowDialogueBoxChat) return; // Không hiển thị nếu đang tương tác

        // Xóa hộp thoại cũ nếu còn tồn tại
        if (currentDialogueBoxChatTransform != null)
        {
            Destroy(currentDialogueBoxChatTransform.gameObject);
            isShowingBoxChat = false;
        }

        DialogueSource randomDialog = dialogueList[Random.Range(0, dialogueList.Count)];

        if (bubbleChatInstance != null)
        {
            currentDialogueBoxChatTransform = bubbleChatInstance.CreateDialogue(transform, new Vector3(0.8f, 2.3f, 0f), randomDialog.icon, randomDialog.text);

            isShowingBoxChat = true;
            lastChatTime = Time.deltaTime;

            StartCoroutine(HideDialogBoxChat(4f));
        }
    }

    IEnumerator HideDialogBoxChat(float time)
    {
        yield return new WaitForSeconds(time);
        if (currentDialogueBoxChatTransform != null)
        {
            Destroy(currentDialogueBoxChatTransform.gameObject);
            currentDialogueBoxChatTransform = null;
        }
        isShowingBoxChat = false;

        // Đợi 2 giây, nếu player vẫn trong phạm vi thì hiển thị lại box chat mới
        yield return new WaitForSeconds(2f);
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectRange)
        {
            ShowRandomDialog();
        }
    }
}
