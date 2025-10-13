using System.Xml.Linq;
using UC;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOptions    _gameOptions;

    static GameManager _instance;

    public static GameManager Instance => _instance;
    public static GameOptions GameOptions => _instance._gameOptions;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GameOptions.dyslexicMode = PlayerPrefsHelpers.GetBool("DyslexicMode", false);
        GameOptions.textScale = PlayerPrefs.GetFloat("SubtitleSize", 1.0f);
    }

    private void Start()
    {
        UpdateFonts();
    }

    static public void UpdateFonts()
    {
        var options = GameManager.GameOptions;
        SubtitleDisplayManager.Instance?.SetFont(options.GetTextFont(), options.GetTextMaterial(), options.textScale);
    }
}
