using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.UI;

public class BubbleChat : MonoBehaviour
{
    public Transform bubbleChatPrefab;
    public Transform CreateDialogue(Transform parent, Vector3 localPosition, Sprite iconType, string text)
    {
        Transform chatBubbleTransform = Instantiate(bubbleChatPrefab, parent);
        chatBubbleTransform.localPosition = localPosition;

        chatBubbleTransform.GetComponent<BubbleChat>().SetUpDialogue(iconType, text);
        
        return chatBubbleTransform;
        //Destroy(chatBubbleTransform.gameObject, 5f);
    }
    public void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        Transform chatBubbleTransform = Instantiate(bubbleChatPrefab, parent);
        chatBubbleTransform.localPosition = localPosition;
        chatBubbleTransform.GetComponent<BubbleChat>().SetUp(iconType, text);

        Destroy(chatBubbleTransform.gameObject, 4f);
    }

    public enum IconType
    {
        Happy, 
        Sad, 
        Angry,
    }

    [SerializeField] private Sprite happyIconSprite;
    [SerializeField] private Sprite sadIconSprite;
    [SerializeField] private Sprite angryIconSprite;

    private SpriteRenderer background;
    private SpriteRenderer icon;
    private TextMeshPro chatText;

    private void Awake()
    {
        background = transform.Find("Background").GetComponent<SpriteRenderer>();
        icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        chatText = transform.Find("TextMP").GetComponent<TextMeshPro>();
    }

    //private void Start()
    //{
    //    SetUp(IconType.Happy, "Hello everybody! Say hello to my little friends!");
    //}

    //private void SetUpDialogue(Sprite iconType, string text)
    //{
    //    chatText.SetText(text);
    //    chatText.ForceMeshUpdate();
    //    Vector2 textSize = chatText.GetRenderedValues(false);

    //    Vector2 padding = new Vector2(5f, 1f);
    //    background.size = textSize + padding;

    //    Vector3 offset = new Vector2(-5f, 0f);
    //    background.transform.localPosition = new Vector3(background.size.x / 2f, 0f) + offset;

    //    icon.sprite = iconType;
    //}

    //*************************
    private void SetUpDialogue(Sprite iconType, string text)
    {
        chatText.SetText(text);
        chatText.ForceMeshUpdate();
        Vector2 textSize = chatText.GetRenderedValues(false);

        Vector2 padding = new Vector2(2f, 1f); // Padding nhỏ để tránh dư thừa
        float iconSize = 1.5f; // Kích thước icon (có thể điều chỉnh nếu icon to)
        float spacing = 0.5f; // Khoảng cách giữa icon và text

        // Tính toán tổng chiều rộng của Bubble Chat (icon + khoảng cách + text + padding)
        float totalWidth = iconSize + spacing + textSize.x + padding.x;
        float totalHeight = Mathf.Max(textSize.y, iconSize) + padding.y;

        // Cập nhật kích thước background
        background.size = new Vector2(totalWidth, totalHeight);

        // Căn giữa hộp thoại trên đầu NPC
        background.transform.localPosition = Vector3.zero;

        // Căn icon sang bên trái hộp thoại
        float iconOffsetX = -totalWidth / 2f + iconSize / 2f;
        icon.transform.localPosition = new Vector3(iconOffsetX, 0f, 0f);

        // Căn text sang bên phải của icon
        float textOffsetX = iconOffsetX + iconSize / 2f + spacing;
        chatText.transform.localPosition = new Vector3(textOffsetX, 0f, 0f);

        // Gán sprite icon
        icon.sprite = iconType;

        // Đặt toàn bộ bubble chat trên đầu NPC
        transform.localPosition = new Vector3(0f, 2.5f, 0f);
    }

    private void SetUp(IconType iconType, string text)
    {
        chatText.SetText(text);
        chatText.ForceMeshUpdate();
        Vector2 textSize = chatText.GetRenderedValues(false);

        Vector2 padding = new Vector2(5f, 2f);
        background.size = textSize + padding;

        Vector3 offset = new Vector2(-5f, 0f);
        background.transform.localPosition = new Vector3(background.size.x / 2f, 0f) + offset;

        icon.sprite = GetIconSprite(iconType);

    }

    private Sprite GetIconSprite(IconType iconType)
    {
        switch (iconType)
        {
            default:
            case IconType.Happy: return happyIconSprite;
            case IconType.Sad: return sadIconSprite;
            case IconType.Angry: return angryIconSprite;

        }
    }

    void Update()
    {
        if (Camera.main != null)
        {
            float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            float scaleFactor = Mathf.Clamp(distance * 0.05f, 0.1f, 0.5f); // Giới hạn scale từ 0.5 đến 1.5

            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
