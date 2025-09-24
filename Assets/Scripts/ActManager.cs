using UnityEngine;
using UC;
using System;

public class ActManager : MonoBehaviour
{
    [SerializeField] private Hypertag[]     activeTags;

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
    }

    private void Update()
    {
        _timeFromStart += Time.deltaTime;
        TriggerEvent(null);
        UpdateFonts();
    }

    public bool IsTagged(Hypertag tag)
    {
        foreach (var t in activeTags)
        {
            if (t == tag) return true;
        }

        return false;
    }

    void TriggerEvent(Trigger trigger)
    {
        var allEventHandlers = FindObjectsByType<OnEvent>(FindObjectsSortMode.None);
        foreach (var handler in allEventHandlers)
        {
            handler.TriggerEvent(trigger);
        }
    }

    void UpdateFonts()
    {
        var options = GameManager.GameOptions;
        SubtitleDisplayManager.Instance?.SetFont(options.GetTextFont(), options.GetTextMaterial(), options.textScale);
    }
}

