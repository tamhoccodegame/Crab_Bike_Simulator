using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBand : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    int lastIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (clips.Length <= 1) return;

            List<AudioClip> availableIndex = new List<AudioClip>();

            for(int i = 0; i < clips.Length; i++)
            {
                if (i != lastIndex) availableIndex.Add(clips[i]);
            }

            int newIndex = Random.Range(0, availableIndex.Count);
            lastIndex = newIndex;

            audioSource.clip = availableIndex[newIndex];
            audioSource.Play();
        }
    }
}
