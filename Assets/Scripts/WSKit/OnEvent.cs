using UnityEngine;

namespace WSKit
{

    public class OnEvent : MonoBehaviour
    {
        [SerializeReference] private Condition[] conditions;
        [SerializeReference] private GameAction[] actions;
        public void TriggerEvent(Condition trigger)
        {
            foreach (var t in conditions)
            {
                if (!t.Evaluate(gameObject))
                {
                    return;
                }
            }

            foreach (var t in conditions)
            {
                t.SetTriggered();
            }

            foreach (var a in actions)
            {
                a.Execute(gameObject);
            }
        }
    }
}