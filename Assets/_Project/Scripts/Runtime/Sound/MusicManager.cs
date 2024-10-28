using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    #region FIELDS

    public static MusicManager Instance;
    public AudioClip mainMenuMusic;
    public AudioClip gameSceneMusic;
    private AudioSource audioSource;
    public AudioClip deathMusic;
    public AudioClip victoryMusic;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #endregion UNITY METHODS

    #region METHODS

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(mainMenuMusic, 0.1f);
                break;

            case "Game":
                PlayMusic(gameSceneMusic, 0.1f);
                break;

            default:
                break;
        }
    }

    private void PlayMusic(AudioClip clip, float volume)
    {
        audioSource.Stop();
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion METHODS
}