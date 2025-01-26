using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public AudioClip[] sounds;

    public float cooldown;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            timer = cooldown;
            AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Length)], Camera.main .transform.position);
        }
    }
}
