using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    private PlayerInteractor currentPlayer;
    public GameObject prompt;
    public KeyCode keyToInteract => KeyCode.E;

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

    // Start is called before the first frame update
    void Start()
    {
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != null && Input.GetKeyDown(keyToInteract))
        {
            if (LightingManager.instance.TimeOfDay >= 22f || LightingManager.instance.TimeOfDay <= 5f)
            {
                SystemNotify.instance.SendNotify("Bạn có muốn ngủ không?", "Ngủ một giấc ngon lành đến sáng!", Sleep, () => { });
            }
            else
            {
                SystemNotify.instance.SendBigNoti("Bạn chỉ có thể ngủ sau 22h", Color.yellow);
            }
        }
    }

    public void Sleep()
    {
        GameManager.instance.ChangeGameState(GameManager.GameState.Sleeping);
    }
}
