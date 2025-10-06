using NaughtyAttributes;
using UnityEngine;

namespace WSKit
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] 
        protected InteractionVerb    interactionVerb;
        [SerializeField] 
        protected bool               overrideCursor;
        [SerializeField, ShowIf(nameof(overrideCursor))] 
        protected CursorDef          cursorDef;

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
        public int priority => interactionVerb.priority;
    }
}
