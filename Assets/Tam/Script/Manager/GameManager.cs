using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject phoneUI;
    public GameObject inventoryUI;

    public GameObject blackScreen;

    public enum GameState
    {
        Playing,
        Sleeping,
        Texting,
        Phone,
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
            if (phoneUI.activeSelf)
            {
                phoneUI.GetComponent<Animator>().SetTrigger("shutdown");
                phoneUI.SetActive(false);

                TPlayerController.instance.canMove = true;
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                ActiveAllController();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                phoneUI.SetActive(true);

                TPlayerController.instance.canMove = false;
                DeactiveAllController();
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
        if (newState == currentState) return;
        currentState = newState;

        switch (currentState)
        {
            case GameState.Sleeping:
                TPlayerController.instance.canMove = false;
                ShowBlackScreen();
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
                if (phoneUI.activeSelf)
                    phoneUI.SetActive(false);
                HideBlackScreen();
                LightingManager.instance.SetDaySpeed(60);
                TPlayerController.instance.canMove = true;
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
    }
}
