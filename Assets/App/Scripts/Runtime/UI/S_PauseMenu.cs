using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_PauseMenu : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Inputs")]
    [SerializeField] private RSE_OnUnpausedGame rse_OnUnpausedGame;
    [SerializeField] private RSE_OnPlayerPause rse_OnPlayerPause;
    [SerializeField] private RSE_OnPauseGame rse_OnPauseGame;

    [Header("Outputs")]
    [SerializeField] private RSE_OnShowCursor rse_OnShowCursor;
    [SerializeField] private RSO_GamePaused rso_GamePaused;
    private bool isPaused = false;

    private void OnEnable()
    {
        rse_OnPlayerPause.action += TogglePause;
        rse_OnUnpausedGame.action += Resume;
        rse_OnPauseGame.action += Pause;
    }
    private void OnDisable()
    {
        rse_OnPlayerPause.action -= TogglePause;
        rse_OnUnpausedGame.action -= Resume;
        rse_OnPauseGame.action -= Pause;
    }
    private void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    private void Resume()
    {
        isPaused = false;
        rso_GamePaused.Value = isPaused;

        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        rse_OnShowCursor.Call(true);
    }
    private void Pause()
    {
        isPaused = true;
        rso_GamePaused.Value = isPaused;

        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        rse_OnShowCursor.Call(false);
    }
}