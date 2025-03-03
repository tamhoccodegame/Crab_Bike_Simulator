using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Health_UI : MonoBehaviour
{
    public Slider npcHealthSlider; // Thanh máu
    //public Transform target;
    private Camera mainCamera;
    //private Vector3 offset = new Vector3(0, 2f, 0); // Vị trí thanh máu trên đầu target

    public GameObject NPCHealth_UI;
    private CanvasGroup canvasGroup; // Điều khiển độ trong suốt để ẩn/hiện thanh máu

    public float showTime;

    private bool hasTakenDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        //npcHealthSlider = GetComponent<Slider>();
        mainCamera = Camera.main;

        canvasGroup = GetComponent<CanvasGroup>(); // Lấy CanvasGroup
        //canvasGroup.alpha = 0; // Mặc định ẩn thanh máu
        //***************
        npcHealthSlider = GetComponentInChildren<Slider>(); // Lấy Slider từ các GameObject con


        NPCHealth_UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (hasTakenDamage) // Chỉ bật/thanh máu nếu NPC đã bị đánh
        //{
        //    //NPCHealth_UI.SetActive(isVisible);
        //    NPCHealth_UI.SetActive(true);
        //    //transform.position = mainCamera.WorldToScreenPoint(target.position + offset);
        //}

        //if (!hasTakenDamage || target == null) return; // Nếu NPC chưa bị đánh, không cập nhật UI

        //// Cố định vị trí UI trên đầu NPC
        //transform.position = target.position + offset;

        //// UI luôn hướng về camera
        ////transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
        ////                 mainCamera.transform.rotation * Vector3.up);

        //if (target != null)
        //{
        //    // Giữ thanh máu đúng trên đầu NPC (không bị lệch theo khoảng cách)
        //    //transform.position = target.position + offset;
        //    // Thanh máu luôn đối diện camera (hướng chính diện)
        //    //transform.forward = Camera.main.transform.forward;

        //    // Chuyển đổi vị trí NPC sang vị trí UI
        //    Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        //    transform.position = screenPos;


        //}
    }

    public void Initialize(Transform newTarget)
    {
        //target = newTarget;
    }

    public void UpdateHeathUI(float currentHealth, float maxHealth)
    {
        StopAllCoroutines();
        StartCoroutine(ShowHealthBar());
        //if (!hasTakenDamage) // Đánh dấu NPC đã bị đánh lần đầu tiên
        //{
        //    hasTakenDamage = true;
        //}

        if (npcHealthSlider != null)
        {
            npcHealthSlider.value = currentHealth / maxHealth;
            //canvasGroup.alpha = 1; // Hiện thanh máu khi NPC bị đánh
        }
    }

    IEnumerator ShowHealthBar()
    {
        NPCHealth_UI.SetActive(true);
        yield return new WaitForSeconds(showTime);
        NPCHealth_UI.SetActive(false);
    }

    public void HideHealthBar()
    {
        gameObject.SetActive(false); // Tắt thanh máu
    }
}
