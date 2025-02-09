using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private int currentAudioIndex;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[0];
        currentAudioIndex = 0;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            int index;
            do
            {
                index = Random.Range(0, audioClips.Length);
            }
            while (index == currentAudioIndex);
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }
}
