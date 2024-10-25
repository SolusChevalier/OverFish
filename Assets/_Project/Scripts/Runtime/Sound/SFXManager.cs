using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    #region FIELDS

    public static SFXManager Instance;

    public AudioClip CargoUpgradeSFX, FishingSpeedUpgradeSFX, BoatSpeedUpgradeSFX, BoatUpgradeSFX, FishSellSfx;
    public AudioClip CatchFishSFX;

    private AudioSource audioSource;

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
    }

    #endregion UNITY METHODS

    #region METHODS

    public void PlaySFX(AudioClip clip)
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(clip);
    }

    #endregion METHODS
}