using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public GameObject VHS_In_Sound;
    private AudioSource vhsSoundAudio;
    
    public GameObject backgroundNoise;
    private AudioSource backgroundNoiseAudio;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        vhsSoundAudio = VHS_In_Sound.GetComponent<AudioSource>();
        backgroundNoiseAudio = backgroundNoise.GetComponent<AudioSource>();
    }

    public void PlayAudioOnce()
    {
        if (!vhsSoundAudio.isPlaying)
        {
            // Play the audio
            vhsSoundAudio.Play();
        }
    }

    public void toggleBackgroundAudioOff()
    {
        backgroundNoiseAudio.mute = true;
    }

    public void toggleBackgroundAudioOn()
    {
        backgroundNoiseAudio.mute = false;
    }
}
