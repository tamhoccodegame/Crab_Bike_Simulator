using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeAdjust : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        float masterDb = PlayerPrefs.GetFloat("MasterVolume", 0.5f); Debug.Log(masterDb);
        float musicDb = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxDb = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        audioMixer.SetFloat("Master", masterDb);
        audioMixer.SetFloat("Music", musicDb);
        audioMixer.SetFloat("SFX", sfxDb);

        masterVolumeSlider.value = Mathf.Pow(10, masterDb / 20);
        musicVolumeSlider.value = Mathf.Pow(10, musicDb / 20);
        sfxVolumeSlider.value = Mathf.Pow(10, sfxDb / 20);
    }

    // Update is called once per frame
    void Update()
    {
        float masterDb = Mathf.Log10(masterVolumeSlider.value) * 20;
        float musicDb = Mathf.Log10(musicVolumeSlider.value) * 20;
        float sfxDb = Mathf.Log10(sfxVolumeSlider.value) * 20;

        audioMixer.SetFloat("Master", masterDb);
        audioMixer.SetFloat("Music", musicDb);
        audioMixer.SetFloat("SFX", sfxDb);

        
            PlayerPrefs.SetFloat("MasterVolume", masterDb);

        
            PlayerPrefs.SetFloat("MusicVolume", musicDb);

       
            PlayerPrefs.SetFloat("SFXVolume", sfxDb);


        PlayerPrefs.Save();
    }
}
