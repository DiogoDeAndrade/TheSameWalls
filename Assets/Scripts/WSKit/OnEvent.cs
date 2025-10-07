using System.Collections;
using UC;
using UnityEngine;

namespace WSKit
{

    public class OnEvent : MonoBehaviour
    {
        [SerializeField] private EventType          _eventType;
        [SerializeField] private GameObject         _referenceObject;
        [SerializeReference] private Condition[]    conditions;
        [SerializeReference] private GameAction[]   actions;

        bool isRunning = false;

        GameObject referenceObject => _referenceObject ? _referenceObject : gameObject;
        public EventType  eventType => _eventType;

        public void _TriggerEvent(EventType evt)
        {
            if (eventType != evt) return;
            if (isRunning) return;

            foreach (var t in conditions)
            {
                if (!t.Evaluate(referenceObject))
                {
                    return;
                }
            }

            foreach (var t in conditions)
            {
                t.SetTriggered();
            }

            StartCoroutine(RunActionsCR());
        }

        IEnumerator RunActionsCR()
        {
            isRunning = true;

            foreach (var a in actions)
            {
                if (a == null)
                    continue;

                // Run the action
                IEnumerator routine = a.Execute(referenceObject);

                if ((a.shouldWait) && (routine != null))
                {
                    // Wait for the coroutine to finish
                    yield return routine;
                }
                else if (routine != null)
                {
                    // Run asynchronously, but don't wait
                    StartCoroutine(routine);
                }
            }

            isRunning = false;
        }

        public static void TriggerEvent(EventType evt)
        {
            var allEventHandlers = FindObjectsByType<OnEvent>(FindObjectsSortMode.None);
            foreach (var handler in allEventHandlers)
            {
                handler._TriggerEvent(evt);
            }
        }
    }
}