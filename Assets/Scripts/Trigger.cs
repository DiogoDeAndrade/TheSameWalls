using UnityEngine;

[System.Serializable]
public abstract class Trigger
{
    public abstract bool ShouldTrigger(Trigger trigger, GameObject go);
}
