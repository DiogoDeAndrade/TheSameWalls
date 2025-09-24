using UnityEngine;

[System.Serializable]
public class Trigger_TimeFromStart : Trigger
{
    [SerializeField] private float elapsedTime = 1.0f;

    public override bool ShouldTrigger(Trigger trigger, GameObject go)
    {
        if (_alreadyTriggered) return false;

        return (actManager.timeFromStart > elapsedTime);
    }
}
