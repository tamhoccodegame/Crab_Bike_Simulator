using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class StartMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SaveLoadManager saveSys = new SaveLoadManager();
        SaveData data = saveSys.LoadGame();
        //if (data != null)
        //{
        loadingScreen.SetActive(true);
        //    StartCoroutine(LoadSceneAsync("MapGame 1"));
        //}
        //else
        {
            StartCoroutine(LoadSceneAsync("StartCutscene"));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadOperation.allowSceneActivation = false;
        while (!loadOperation.isDone)
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
        loadingScreen.SetActive(false);
    }
}
