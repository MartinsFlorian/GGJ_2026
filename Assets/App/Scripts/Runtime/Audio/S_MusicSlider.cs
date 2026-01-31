using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class S_MusicSlider : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    [Header("Inputs")]
    [SerializeField] private RSE_OnSetMusicVolume onSetMusicVolume;

    //[Header("Outputs")]
    private void OnEnable()
    {
        onSetMusicVolume.action += SetMusicVolume;
    }
    private void OnDisable()
    {
        onSetMusicVolume.action -= SetMusicVolume;
    }
    private void Start()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
    }
    private void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SetMusicVolume();
    }
}