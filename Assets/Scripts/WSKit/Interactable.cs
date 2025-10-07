using NaughtyAttributes;
using UnityEngine;

namespace WSKit
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] 
        protected InteractionVerb   interactionVerb;
        [SerializeField]
        protected int               _priority = 0;
        [SerializeField] 
        protected bool              overrideCursor;
        [SerializeField, ShowIf(nameof(overrideCursor))] 
        protected CursorDef         cursorDef;
        [SerializeReference]
        protected Condition[]       conditions;
        [SerializeReference]
        protected GameAction[]      actions;
        [SerializeField]
        protected float             cooldown = 2.0f;
        [SerializeField]
        protected bool              canRetrigger = true;

        float lastInteractionTime = float.NegativeInfinity;

        public CursorDef cursor
        {
            get
            {
                if (overrideCursor)
                {
                    return cursorDef;
                }
                return interactionVerb.cursorDef;
            }
        }
        public int priority => _priority;

        public virtual bool CanInteract(GameObject referenceObject)
        {
            if ((!canRetrigger) && (lastInteractionTime >= 0)) return false;
            if (cooldown > 0.0f)
            {
                return (Time.time - lastInteractionTime) > cooldown;
            }

            return true;
        }

        public bool Interact(GameObject referenceObject)
        {
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    if (!condition.Evaluate(referenceObject)) return false;
                }
            }

            foreach (var action in actions)
            {
                action.Execute(referenceObject);
            }

            lastInteractionTime = Time.time;

            return true;
        }
    }
}
