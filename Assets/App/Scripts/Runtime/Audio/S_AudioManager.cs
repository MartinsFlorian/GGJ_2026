using UnityEngine;

public class S_AudioManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    //[Header("Inputs")]

    //[Header("Outputs")]

    private void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
}