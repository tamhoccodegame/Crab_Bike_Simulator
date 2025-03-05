using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject phoneUI;
    public GameObject inventoryUI;
    public GameObject pauseUI;

    [Header("Loading Session")]
    public GameObject loadingScreen;
    public Slider loadingSlider;

    public event Action onScenePreLoad;
    public event Action<SaveData> onSceneLoaded;

    public GameObject blackScreen;

    public AudioMixer audioMixer;

    public Vector3 playerSavedPosition;

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
                LightingManager.instance.SetDaySpeed(3);
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
                LightingManager.instance.SetDaySpeed(25); //Nhớ chỉnh lại, 25 là demo thôi, real là 60
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
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        SaveLoadManager saveSys = new SaveLoadManager();
        SaveData data = saveSys.LoadGame();
        playerSavedPosition = data.playerPosition;
        onScenePreLoad?.Invoke();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        loadOperation.allowSceneActivation = false;
        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            if (loadOperation.progress >= 0.9f) // Scene đã load xong
            {
                yield return new WaitForSeconds(4f); // Chờ 1 giây để chắc chắn mọi thứ đã khởi tạo
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        controllerList.Clear();

        onSceneLoaded?.Invoke(data);
        loadingScreen.SetActive(false);

        ChangeGameState(GameState.Playing);

    }
}
