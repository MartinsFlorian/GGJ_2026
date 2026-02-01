using UnityEngine;

public class S_PlayerHealth : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerTakeDamage onPlayerTakeDamage;

    [Header("Outputs")]
    [SerializeField] private RSE_OnShowPanel onShowLosePanel;
    [SerializeField] private RSE_OnShowCursor rse_OnShowCursor;
    [SerializeField] private RSE_OnPauseGame rse_OnPauseGame;

    private void OnEnable()
    {
        onPlayerTakeDamage.action += HandleTakeDamage;
    }
    private void OnDisable()
    {
        onPlayerTakeDamage.action -= HandleTakeDamage;
    }
    private void HandleTakeDamage()
    {
        rse_OnShowCursor.Call(false);
        onShowLosePanel.Call(true);
        rse_OnPauseGame.Call();
    }
}