using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip soundClip;
    [Range(0, 1f)]
    public float volume;
    [Range(0, 2f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer audioMixer;

    public List<Sound> sounds;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.soundClip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string soundName)
    {
        Sound sound = sounds.Find(s => s.soundName == soundName);
        if (sound != null)
        {
            sound.source.Play();
        }
    }
}
