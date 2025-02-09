using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OnCutsceneEnd : MonoBehaviour
{
    private PlayableDirector cutscene;
    // Start is called before the first frame update
    void Start()
    {
        cutscene = GetComponent<PlayableDirector>();
        cutscene.stopped += Cutscene_stopped;
    }

    private void Cutscene_stopped(PlayableDirector obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
