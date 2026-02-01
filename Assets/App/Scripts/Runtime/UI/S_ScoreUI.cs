using TMPro;
using UnityEngine;

public class S_ScoreUI : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Inputs")]
    [SerializeField] private RSO_PlayerScore rso_PlayerScore;

    //[Header("Outputs")]
    private void OnEnable()
    {
        rso_PlayerScore.onValueChanged += UpdateText;
    }
    private void OnDisable()
    {
        rso_PlayerScore.onValueChanged -= UpdateText;
    }
    private void UpdateText(float value)
    {
        value = Mathf.FloorToInt(value);
        scoreText.text = $"Score: {value.ToString()}";
    }
}