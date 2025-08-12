using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource fireSingularSound, fireMultipleSound, menuTrack, gameTrack1, gameOverTrack, teleporterActive, teleporterUsed, winSound, winMusic;
    public Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    void Awake()
    {
        instance = this;
        audioSources.Add("fireSingularSFX", fireSingularSound);
        audioSources.Add("fireMultipleSFX", fireMultipleSound);
        audioSources.Add("menuTrack", menuTrack);
        audioSources.Add("gameTrack1", gameTrack1);
        audioSources.Add("gameOverTrack", gameOverTrack);
        audioSources.Add("teleporterActiveSFX", teleporterActive);
        audioSources.Add("teleporterUsedSFX", teleporterUsed);
        audioSources.Add("winTrack", winMusic);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            PlaySound(audioSources["menuTrack"]);
        }
        if (SceneManager.GetActiveScene().name == "GameSceneNorm")
        {
            PlaySound(audioSources["gameTrack1"]);
        }
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            PlaySound(audioSources["gameOverTrack"]);
        }
    }

    public void PlaySound(AudioSource audio)
    {
        if (audio == audioSources["fireSingularSFX"] || audio == audioSources["fireMultipleSFX"])
        {
            audio.pitch = Random.Range(0.5f, 1.5f);
        }
        audio.volume = (GameData.sfxVolume / 100);
        audio.Play();
    }

    public void PlayMusic(AudioSource audio)
    {
        audio.volume = (GameData.musicVolume / 100);
        audio.Play();   
    }

    public void StopPlaying(AudioSource audio)
    {
        audio.Stop();
    }

    public void UpdateVolume()
    {
        foreach (KeyValuePair<string, AudioSource> pair in audioSources)
        {
            AudioSource audio = pair.Value;
            string audioString = pair.Key;
            if (audio.isPlaying)
            {
                if (audioString.Contains("Track"))
                {
                    audio.volume = (GameData.musicVolume / 100);
                }
                else if (audioString.Contains("SFX"))
                {
                    audio.volume = (GameData.sfxVolume / 100);
                }
            }
        }
    }
}
