using UnityEngine;

public class S_PauseMenu : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowPanel onShowPauseMenu;

    //[Header("Outputs")]
    private void OnEnable()
    {
        onShowPauseMenu.action += ShowPauseMenu;
    }
    private void OnDisable()
    {
        onShowPauseMenu.action -= ShowPauseMenu;
    }
    private void ShowPauseMenu(bool show)
    {
        pauseMenuUI.SetActive(show);
        if(show)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        } 
    }
}