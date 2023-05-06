using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource[] soundEffects;
    public AudioSource bgmusic, levelEndMusic, bossMusic;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int sound)
    {
        // Disabling sound overlaying
        soundEffects[sound].Stop();

        // Adding variation to the sound
        soundEffects[sound].pitch = Random.Range(.9f, 1.1f);
        soundEffects[sound].Play();
    }

    public void PlayLevelVictory()
    {
        bgmusic.Stop();
        levelEndMusic.Play();
    }

    public void PlayBossMusic()
    {
        bgmusic.Stop();
        bossMusic.Play();
    }

    public void StopBossMusic()
    {
        bossMusic.Stop();
        bgmusic.Play();
    }
}
