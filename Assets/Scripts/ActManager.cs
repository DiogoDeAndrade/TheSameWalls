using UnityEngine;
using UC;

public class ActManager : MonoBehaviour
{
    [SerializeField] private WSKit.EventType    startEvent;
    [SerializeField] private Hypertag[]     activeTokens;

    private float _timeFromStart = 0.0f;

    public static ActManager Instance;
    public float timeFromStart => _timeFromStart;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _timeFromStart = 0.0f;

        UpdateFonts();

        var tokenManager = FindFirstObjectByType<TokenManager>();
        foreach (var t in activeTokens)
            tokenManager?.Add(t);
    }

    private void Update()
    {
        _timeFromStart += Time.deltaTime;
        WSKit.OnEvent.TriggerEvent(startEvent);
        UpdateFonts();
    }

    void UpdateFonts()
    {
        var options = GameManager.GameOptions;
        SubtitleDisplayManager.Instance?.SetFont(options.GetTextFont(), options.GetTextMaterial(), options.textScale);
    }
}

