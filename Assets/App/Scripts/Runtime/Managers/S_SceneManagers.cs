using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneManagers : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnLoadScene onLoadScene;
    [SerializeField] private RSE_OnQuitGame onQuitGame;

    //[Header("Outputs")]

    private void OnEnable()
    {
        onLoadScene.action += LoadScene;
        onQuitGame.action += QuitGame;
    }
    private void OnDisable()
    {
        onLoadScene.action -= LoadScene;
        onQuitGame.action -= QuitGame;
    }

    private void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}