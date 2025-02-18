using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemNotify : MonoBehaviour
{
    public static SystemNotify instance;
    public GameObject systemNotiPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public Button yesButton;
    public Button noButton;

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
        systemNotiPanel.SetActive(true);
        title.text = _title; 
        content.text = _content;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        
        yesButton.onClick.AddListener(() =>
        {
            yesAction?.Invoke();
            systemNotiPanel.SetActive(false);
        });
        noButton.onClick.AddListener(() =>
        {
            noAction?.Invoke();
            systemNotiPanel.SetActive(false);
        });
    }
}
