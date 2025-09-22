using UnityEngine;

[System.Serializable]
public class Trigger_Start : Trigger
{
    public override bool ShouldTrigger(Trigger trigger, GameObject go)
    {
        return (trigger.GetType() == typeof(Trigger_Start));
    }
}
