using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject phoneUI;
    public GameObject inventoryUI;
    public GameObject pauseUI;

    public GameObject blackScreen;

    public AudioMixer audioMixer;

    public enum GameState
    {
        Playing,
        Sleeping,
        Texting,
        Menu,
    }

    public GameState currentState;
    public bool canChangeGameState = true;

    public TextMeshProUGUI fpsText;

    private List<Controller> controllerList = new List<Controller>();

    private void Awake()
    {
        if (instance == null)
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
        InvokeRepeating(nameof(UpdateFPS), 1f, 1f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateFPS()
    {
        fpsText.text = (1f / Time.deltaTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!phoneUI.activeSelf)
            {
                phoneUI.SetActive(true);
                ChangeGameState(GameState.Menu);
            }
            else
            {
                ChangeGameState(GameState.Playing);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
                ChangeGameState(GameState.Playing);
            }
            else
            {
                inventoryUI.SetActive(true);
                ChangeGameState(GameState.Menu);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeSelf)
            {
                pauseUI.SetActive(false);
                ChangeGameState(GameState.Playing);
            }
            else
            {
                pauseUI.SetActive(true);
                ChangeGameState(GameState.Menu);
            }
        }
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == currentState) return;
        currentState = newState;

        switch (currentState)
        {
            case GameState.Sleeping:
                TPlayerController.instance.canMove = false;
                StartCoroutine(BlackScreenCoroutine(25f));
                LightingManager.instance.SetDaySpeed(5);
                break;

            case GameState.Texting:
                TPlayerController.instance.canMove = false;
                HideBlackScreen();
                LightingManager.instance.SetDaySpeed(80);
                phoneUI.SetActive(true);
                SMSSystem.instance.StartShowSMS();
                break;

            case GameState.Playing:
                phoneUI.SetActive(false);
                pauseUI.SetActive(false);
                inventoryUI.SetActive(false);
                LightingManager.instance.SetDaySpeed(60);
                TPlayerController.instance.canMove = true;
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ActiveAllController();
                break;

            case GameState.Menu:
                TPlayerController.instance.canMove = false;
                DeactiveAllController();
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

        }
    }

    void ActiveAllController()
    {
        foreach (var c in controllerList)
        {
            c.enabled = true;
        }
        controllerList.Clear();
    }

    void DeactiveAllController()
    {
        var controllers = FindObjectsOfType<Controller>();
        foreach (var c in controllers)
        {
            if (!c.enabled) continue;
            c.enabled = false;
            controllerList.Add(c);
        }
    }

    public void SetCanChangeGameState(bool _canChangeGameState)
    {
        canChangeGameState = _canChangeGameState;
    }

    public void ShowBlackScreen()
    {
        blackScreen.SetActive(true);
    }

    public void HideBlackScreen()
    {
        blackScreen.SetActive(false);
    }

    public void ShowBlackScreen(float time)
    {
        blackScreen.SetActive(true);
    }

    IEnumerator BlackScreenCoroutine(float time)
    {
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        blackScreen.SetActive(false);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        SaveLoadManager saveSys = new SaveLoadManager();
        saveSys.SaveGame();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        StartCoroutine(BlackScreenCoroutine(5f));
        SaveLoadManager saveSys = new SaveLoadManager();
        SaveData data = saveSys.LoadGame();

        TPlayerController.instance.GetComponent<CharacterController>().enabled = false;
        TPlayerController.instance.transform.position = data.playerPosition;
        TPlayerController.instance.GetComponent<CharacterController>().enabled = true;
        PlayerCash.instance.currentCash = data.playerCash;

        List<InventoryReplace> replaces = data.inventoryItems;
        foreach (InventoryReplace replace in replaces)
        {
            if (replace.typeName == "Food")
            {
                Food.FoodType type;
                if (Enum.TryParse(replace.itemName, out type))
                    PlayerInventory.instance.AddItem(new Food { foodType = type });
            }
            else if (replace.typeName == "Furniture")
            {
                Furniture.FurnitureType type;
                if (Enum.TryParse(replace.itemName, out type))
                    PlayerInventory.instance.AddItem(new Furniture { type = type });
            }
        }


        VehicleManager vehicleManager = VehicleManager.instance;
        vehicleManager.ownVehicles.Clear();
        foreach (string vehicleName in data.ownVehiclesName)
        {
            GameObject prefab = vehicleManager.allVehiclesPrefab.Find(v => v.name == vehicleName);
            if (prefab != null)
            {
                vehicleManager.ownVehicles.Add(prefab);
            }
        }

        House[] allHouses = FindObjectsOfType<House>();
        foreach (House house in allHouses)
        {
            if (house.id == data.ownHouseId)
            {
                HouseManager h = HouseManager.instance;
                h.currentOwnHouse = house;
                h.currentOwnHouse.isOwned = true;
                h.currentOwnHouse.houseDoor.enabled = true;
                break;
            }
        }
        ChangeGameState(GameState.Playing);
    }
}
