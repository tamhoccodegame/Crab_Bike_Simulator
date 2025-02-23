using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemNotify : MonoBehaviour
{
    public static SystemNotify instance;

    [Header("YesNoNotify")]
    public GameObject systemNotiPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public Button yesButton;
    public Button noButton;

    [Header("MiniNotify")]
    public GameObject m_SystemNotiPanel;
    public TextMeshProUGUI m_title;
    public TextMeshProUGUI m_content;

    [Header("BigTextNoti")]
    public TextMeshProUGUI bigText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendNotify(string _title, string _content, Action yesAction, Action noAction)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        systemNotiPanel.SetActive(true);
        title.text = _title; 
        content.text = _content;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        
        yesButton.onClick.AddListener(() =>
        {
            yesAction?.Invoke();
            systemNotiPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        });
        noButton.onClick.AddListener(() =>
        {
            noAction?.Invoke();
            systemNotiPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        });
    }

    public void SendMNotify(string _title, string _content)
    {
        AudioManager.instance.PlaySound("UI_NotifyPopUp");
        m_SystemNotiPanel.SetActive(true);
        m_title.text = _title;
        m_content.text = _content;
        StartCoroutine(TurnOffPanel());
    }

    public void SendBigNoti(string content, Color color)
    {
        bigText.text = content;
        bigText.color = color;
        bigText.gameObject.SetActive(true);
        StartCoroutine(TurnOffPanel());
    }

    IEnumerator TurnOffPanel()
    {
        yield return new WaitForSeconds(3f);
        m_SystemNotiPanel.SetActive(false);
        bigText.gameObject.SetActive(false);
    }
}
