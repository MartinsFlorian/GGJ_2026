using UnityEngine;

public class S_MainMenuUI : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject mainMenuUI;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowPanel onShowMainMenu;

    //[Header("Outputs")]

    private void OnEnable()
    {
        onShowMainMenu.action += MainMenu;
    }
    private void OnDisable()
    {
        onShowMainMenu.action -= MainMenu;
    }
    private void MainMenu(bool show)
    {
        mainMenuUI.SetActive(show);
    }
}