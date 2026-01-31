using TMPro;
using UnityEngine;

public class S_GameOver : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_ListGameOverText listGameOverText;

    [Header("References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowPanel onShowGameOver;

    //[Header("Outputs")]

    private void OnEnable()
    {
        onShowGameOver.action += GameOver;
    }
    private void OnDisable()
    {
        onShowGameOver.action -= GameOver;
    }
    private void GameOver(bool show)
    {
        gameOverUI.SetActive(show);
        if (show)
        {
            finalScoreText.text = "Final Score: ";
            GetRandomText();
        }
    }

    private void GetRandomText()
    {
        int randomIndex = Random.Range(0, listGameOverText.Value.Count);
        gameOverText.text = listGameOverText.Value[randomIndex];
    }
}