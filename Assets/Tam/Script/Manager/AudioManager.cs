using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip soundClip;
    [Range(0, 1f)]
    public float volume;
    [Range(0, 1f)]
    public float pitch;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
