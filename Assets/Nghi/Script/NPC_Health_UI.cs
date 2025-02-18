using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Health_UI : MonoBehaviour
{
    public Slider npcHealthSlider; // Thanh máu
    [SerializeField] private GameObject NPCHealth_UI;
    public Transform target;

    private Vector3 offset = new Vector3(0, 2f, 0); // Vị trí thanh máu trên đầu target
    // Start is called before the first frame update
    void Start()
    {
        npcHealthSlider = GetComponent<Slider>();
        NPCHealth_UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;

            //// Chuyển đổi vị trí NPC sang vị trí UI
            //Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
            //transform.position = screenPos; 

            transform.forward = Camera.main.transform.forward;
        }
    }

    public void Initialize(Transform newTarget)
    {
        target = newTarget;
    }

    public void UpdateHeathUI(float currentHeath, float maxHealth)
    {
        NPCHealth_UI.SetActive(true);
        npcHealthSlider.value = currentHeath / maxHealth;
    }
}
