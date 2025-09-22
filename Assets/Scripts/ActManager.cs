using UnityEngine;
using UC;
using System;

public class ActManager : MonoBehaviour
{
    [SerializeField] private Hypertag[] activeTags;

    public static ActManager Instance;

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
        TriggerEvent(new Trigger_Start());
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
}
