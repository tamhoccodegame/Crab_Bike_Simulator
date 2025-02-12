using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject phoneUI;
    public GameObject inventoryUI;

    public enum GameState
    {
        Playing,
        Sleeping,
        Texting,
    }

    public GameState currentState;
    public bool canChangeGameState = true;

	private void Awake()
	{
		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if(Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (phoneUI.activeSelf)
            {
                phoneUI.SetActive(false);
            }
            else
            {
                phoneUI.SetActive(true);
			}
		}

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
            }
            else
            {
                inventoryUI.SetActive(true);
            }
        }
    }

    public void ChangeGameState(GameState newState)
    {
        if (!canChangeGameState) return;
        canChangeGameState = false;
        currentState = newState;

        switch(currentState)
        {
            case GameState.Sleeping:
                TPlayerController.instance.canMove = false;
                LightingManager.instance.SetDaySpeed(5);
                break;
            case GameState.Texting:
                TPlayerController.instance.canMove = false;
                LightingManager.instance.SetDaySpeed(3600);
                SMSSystem.instance.StartShowSMS();
                break;
            case GameState.Playing:
                LightingManager.instance.SetDaySpeed(60);
                TPlayerController.instance.canMove = true;
                break;
        }
    }

    public void SetCanChangeGameState(bool _canChangeGameState)
    {
        canChangeGameState = _canChangeGameState;
    }
}
