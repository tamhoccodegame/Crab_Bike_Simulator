using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.UI;

public class BubbleChat : MonoBehaviour
{
    public Transform bubbleChatPrefab;
    public void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        Transform chatBubbleTransform = Instantiate(bubbleChatPrefab, parent);
        chatBubbleTransform.localPosition = localPosition;
        chatBubbleTransform.GetComponent<BubbleChat>().SetUp(iconType, text);
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

    private void Start()
    {
        SetUp(IconType.Happy, "Hello everybody! Say hello to my little friends!");
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
        switch(iconType)
        {
            default:
            case IconType.Happy: return happyIconSprite;
            case IconType.Sad: return sadIconSprite;
            case IconType.Angry: return angryIconSprite;
                
        }
    }

}
