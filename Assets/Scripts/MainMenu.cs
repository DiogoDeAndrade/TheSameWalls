using NaughtyAttributes;
using UC;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Scene] private string room01;
    [SerializeField] private BigTextScroll creditsScroll;
    [SerializeField] private CanvasGroup menuCanvasGroup;

    public void StartGame()
    {
        SceneManager.LoadScene(room01);
    }

    public void ShowCredits()
    {
        menuCanvasGroup.FadeOut(0.5f);

        var creditsCanvasGroup = creditsScroll.GetComponent<CanvasGroup>();
        creditsCanvasGroup.FadeIn(0.5f);

        creditsScroll.Reset();

        creditsScroll.onEndScroll += CreditsScroll_onEndScroll;
    }

    private void CreditsScroll_onEndScroll()
    {
        menuCanvasGroup.FadeIn(0.5f);

        var creditsCanvasGroup = creditsScroll.GetComponent<CanvasGroup>();
        creditsCanvasGroup.FadeOut(0.5f);

        creditsScroll.onEndScroll -= CreditsScroll_onEndScroll;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
