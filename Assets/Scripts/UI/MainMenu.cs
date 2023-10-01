using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject credits;

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMainPage()
    {
        credits.SetActive(false);
        mainPage.SetActive(true);
    }

    public void OpenCredits()
    {
        credits.SetActive(true);
        mainPage.SetActive(false);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
