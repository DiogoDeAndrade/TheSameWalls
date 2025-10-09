using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycastPeek : MonoBehaviour
{
    readonly List<RaycastResult> _results = new();

    void Update()
    {
        if (EventSystem.current == null) return;

        var ped = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results.Clear();
        EventSystem.current.RaycastAll(ped, _results);

        if (_results.Count == 0) return;

        // Log the topmost hit each frame (or uncomment to list all)
        var top = _results[0];
        Debug.Log($"UI hit: {top.gameObject.name} (sortingOrder {top.sortingOrder})");
        // foreach (var r in _results) Debug.Log($" - {r.gameObject.name}");
    }
}
