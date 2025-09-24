using UnityEngine;

public class OnEvent : MonoBehaviour
{
    [SerializeReference] private Trigger[]      triggers;
    [SerializeReference] private GameAction[]   actions;
    public void TriggerEvent(Trigger trigger)
    {
        foreach (var t in triggers)
        {
            if (!t.ShouldTrigger(trigger, gameObject))
            {
                return;
            }
        }

        foreach (var t in triggers)
        {
            t.SetTriggered();
        }

        foreach (var a in actions)
        {
            a.Execute(gameObject);
        }
    }   
}
