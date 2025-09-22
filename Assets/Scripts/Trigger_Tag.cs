using UC;
using UnityEngine;

[System.Serializable]
public class Trigger_Tag : Trigger
{
    [SerializeField] private Hypertag tag;
    public override bool ShouldTrigger(Trigger trigger, GameObject go)
    {
        if (ActManager.Instance == null) return false;

        if (ActManager.Instance.IsTagged(tag)) return true;

        return false;
    }
}
