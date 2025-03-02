using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSaleSign : MonoBehaviour, IInteractable
{
    private PlayerInteractor currentPlayer;

    public KeyCode keyToInteract => KeyCode.E;

    public GameObject prompt;

    public void ResetInteractState()
    {
        currentPlayer = null;
        prompt.SetActive(false);
    }

    public void ShowPrompt(PlayerInteractor player)
    {
        currentPlayer = player;
        prompt.SetActive(true);
    }

    void BuyHouse()
    {
        if (HouseManager.instance.BuyHouse(GetComponentInParent<House>()))
        {
            this.gameObject.SetActive(false);
            currentPlayer = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (prompt != null && prompt.activeSelf)
        {
            Vector3 direction = Camera.main.transform.position - prompt.transform.position;
            direction.y = 0; // Giữ nguyên trục Y để không bị nghiêng
            prompt.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            SystemNotify.instance.SendNotify($"Mua nhà", "Bạn có chắc muốn mua căn nhà này với giá {asd}", BuyHouse, () => { });
        }
    }
}
