using System.Collections;
using UnityEngine;

public class S_ScoreManager : MonoBehaviour
{
    //[Header("Settings")]
    private float scoreMultiplier = 1f;
    [SerializeField] private float scoreIncreaseRate = 10f;
    [SerializeField] private float multiplierAmount = 0.5f;
    [SerializeField] private float multiplierIncreaseInterval = 30f;
    [Header("References")]
    [SerializeField] private RSO_PlayerScore playerScore;
    private float score = 0f;

    //[Header("Inputs")]

    //[Header("Outputs")]
    private Coroutine increaseMultiplierCoroutine;
    private void OnEnable()
    {
        score = playerScore.Value;
    }
    private void OnDisable()
    {
        playerScore.Value = score;
    }
    private void Start()
    {
        if(increaseMultiplierCoroutine != null)
        {
            StopCoroutine(increaseMultiplierCoroutine);
            increaseMultiplierCoroutine = null;
        }
        increaseMultiplierCoroutine = StartCoroutine(IncreaseMultiplier());
    }
    private void Update()
    {
        IncreaseScore();
    }
    private void IncreaseScore()
    {
        playerScore.Value += (scoreIncreaseRate * scoreMultiplier) * Time.deltaTime;
    }

    private IEnumerator IncreaseMultiplier()
    {
        yield return new WaitForSeconds(multiplierIncreaseInterval);
        scoreMultiplier += multiplierAmount;

        if (increaseMultiplierCoroutine != null)
        {
            StopCoroutine(increaseMultiplierCoroutine);
            increaseMultiplierCoroutine = null;
        }
        increaseMultiplierCoroutine = StartCoroutine(IncreaseMultiplier());
    }
}