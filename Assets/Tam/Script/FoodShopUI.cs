using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShopUI : MonoBehaviour, IInteractable
{
    public GameObject shopUI;
    public Transform shopItemTemplate;
    private PlayerInteractor currentPlayer;

    public void OnExit()
    {
        shopUI.SetActive(false);
        currentPlayer.GetComponent<Animator>().SetBool("Shopping", false);
        currentPlayer = null;
    }

    public void OnInteract(PlayerInteractor player)
    {
        shopUI.SetActive(true);
        currentPlayer = player;
        currentPlayer.GetComponent<Animator>().SetBool("Shopping", true);
    }

    public void ShowPrompt()
    {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
