using UnityEngine;

public class S_Credits : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject creditsUI;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowPanel onShowCredits;

    //[Header("Outputs")]
    private void OnEnable()
    {
        onShowCredits.action += Credits;
    }
    private void OnDisable()
    {
        onShowCredits.action -= Credits;
    }
    private void Credits(bool show)
    {
        creditsUI.SetActive(show);
    }
}