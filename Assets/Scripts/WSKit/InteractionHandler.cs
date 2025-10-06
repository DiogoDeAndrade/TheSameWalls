using System;
using UnityEngine;

namespace WSKit
{ 
    public class InteractionHandler : MonoBehaviour
    {
        [SerializeField] private bool       interactionEnabled = true;
        [SerializeField] private float      interactionRange = 10.0f;
        [SerializeField] private float      raycastRadius = 0.5f;
        [SerializeField] private LayerMask  interactionLayers;

        CursorManager cursorManager;

        private void Start()
        {
            cursorManager = FindFirstObjectByType<CursorManager>();
        }

        private void Update()
        {
            if (!interactionEnabled) return;

            var hits = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, interactionRange, interactionLayers);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            Interactable[]  interactables = null;
            Interactable    activeInteraction = null;

            foreach (var hit in hits)
            {
                interactables = hit.collider.GetComponentsInChildren<Interactable>();
                if (interactables.Length > 0)
                {
                    break;
                }
            }

            if ((interactables != null) && (interactables.Length > 0))
            {
                Array.Sort(interactables, (a, b) => b.priority.CompareTo(a.priority));

                activeInteraction = interactables[0];
            }

            if (activeInteraction)
            { 
                // Handle cursor
                if (cursorManager)
                {
                    cursorManager.SetCursor(activeInteraction.cursor);
                }
            }
            else
            {
                if (cursorManager)
                {
                    cursorManager.SetCursor(null);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
