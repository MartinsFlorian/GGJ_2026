using UnityEngine;

public class S_Settings : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject settingsUI;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowPanel onShowSettings;

    //[Header("Outputs")]

    private void OnEnable()
    {
        onShowSettings.action += Settings;
    }
    private void OnDisable()
    {
        onShowSettings.action -= Settings;
    }
    private void Settings(bool show)
    {
        settingsUI.SetActive(show);
    }
}