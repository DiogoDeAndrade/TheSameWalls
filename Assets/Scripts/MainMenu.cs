using NaughtyAttributes;
using UC;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Scene] private string room01;
    [SerializeField] private BigTextScroll creditsScroll;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private CanvasGroup optionsCanvasGroup;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 cursorHotspot;
    [SerializeField] private SoundDef voiceTestSound;
    [SerializeField] private SoundDef subtitleTestSound;
    [SerializeField] private GameOptions gameOptions;

    AudioSource voiceTestAudioSource;
    AudioSource subtitleTestAudioSource;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    public void StartGame()
    {
        menuCanvasGroup.FadeOut(0.5f);

        FullscreenFader.FadeOut(2.0f, Color.black, () =>
        {
            SceneManager.LoadScene(room01);
        });
    }

    public void ShowOptions()
    {
        menuCanvasGroup.FadeOut(0.5f);

        optionsCanvasGroup.FadeIn(0.5f);
    }

    public void OptionsToMenu()
    {
        menuCanvasGroup.FadeIn(0.5f);

        optionsCanvasGroup.FadeOut(0.5f);
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
        SoundManager.PlayMusic(null, 0.0f, 0.0f, 2.0f);
        FullscreenFader.FadeOut(2.0f, Color.black, () =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    public void OnMusicVolumeChange(Slider slider)
    {
        SoundManager.SetVolume(SoundType.Music, slider.value, true);
    }

    bool firstTriggerVoiceTest = false;

    public void OnVoiceVolumeChange(Slider slider)
    {
        float prevValue = SoundManager.GetVolume(SoundType.Voice);
        if (prevValue == slider.value) return;

        SoundManager.SetVolume(SoundType.Voice, slider.value, true);

        if (!firstTriggerVoiceTest)
        {
            firstTriggerVoiceTest = true;
            return;
        }

        if ((voiceTestAudioSource != null) && (voiceTestAudioSource.isPlaying))
        {
            // Do nothing
        }
        else
        {
            voiceTestAudioSource = voiceTestSound.Play();
        }
    }

    public void OnGammaChange(Slider slider)
    {
        PlayerPrefs.SetString("Gamma", $"1.0;1.0;1.0;{slider.value}");
        PlayerPrefs.Save();
    }

    bool firstTriggerDyslexiaMode = false;
    public void OnDyslexiaModeToggle(Toggle toggle)
    {
        gameOptions.dyslexicMode = toggle.isOn;
        PlayerPrefsHelpers.SetBool("DyslexicMode", gameOptions.dyslexicMode);
        PlayerPrefs.Save();
        GameManager.UpdateFonts();

        if (!firstTriggerDyslexiaMode)
        {
            firstTriggerDyslexiaMode = true;
            return;
        }

        if ((subtitleTestAudioSource != null) && (subtitleTestAudioSource.isPlaying))
        {
            subtitleTestAudioSource.Stop();
        }

        subtitleTestAudioSource = subtitleTestSound.Play();
    }

    bool firstTriggerSubtitleSize = false;
    public void OnSubtitleSizeChange(Slider slider)
    {
        gameOptions.textScale = slider.value;

        PlayerPrefs.SetFloat("SubtitleSize", slider.value);
        PlayerPrefs.Save();
        GameManager.UpdateFonts();

        if (!firstTriggerSubtitleSize)
        {
            firstTriggerSubtitleSize = true;
            return;
        }

        if ((subtitleTestAudioSource != null) && (subtitleTestAudioSource.isPlaying))
        {
            subtitleTestAudioSource.Stop();
        }

        subtitleTestAudioSource = subtitleTestSound.Play();
    }
}
