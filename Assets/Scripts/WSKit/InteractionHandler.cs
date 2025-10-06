using System;
using UC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WSKit
{ 
    public class InteractionHandler : MonoBehaviour
    {
        [SerializeField] 
        private bool               interactionEnabled = true;
        [SerializeField] 
        private GameObject         masterInteractionObject;
        [SerializeField] 
        private float              interactionRange = 10.0f;
        [SerializeField] 
        private float              raycastRadius = 0.5f;
        [SerializeField] 
        private LayerMask          interactionLayers;
        [SerializeField] 
        private PlayerInput        playerInput;
        [SerializeField, InputPlayer(nameof(playerInput)), InputButton] 
        private UC.InputControl    interactControl;

        CursorManager cursorManager;
        GameObject    referenceObject => masterInteractionObject ? masterInteractionObject : gameObject;

        private void Start()
        {
            cursorManager = FindFirstObjectByType<CursorManager>();
            interactControl.playerInput = playerInput;
        }

        private void Update()
        {
            if (!interactionEnabled) return;

            var hits = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, interactionRange, interactionLayers);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            Interactable[] interactables = null;
            Interactable activeInteraction = null;

            foreach (var hit in hits)
            {
                var localInteractables = hit.collider.GetComponentsInChildren<Interactable>();
                if (localInteractables.Length > 0)
                {
                    Array.Sort(localInteractables, (a, b) => b.priority.CompareTo(a.priority));

                    // Check if any is interactable
                    foreach (var i in localInteractables)
                    {
                        if (i.CanInteract(referenceObject))
                        {
                            activeInteraction = i;
                            break;
                        }

                        if (activeInteraction)
                        {
                            interactables = localInteractables;
                            break;
                        }
                    }
                }
            }

            if (activeInteraction)
            {
                // Handle cursor
                if (cursorManager)
                {
                    cursorManager.SetCursor(activeInteraction.cursor);
                }

                // Interact
                if (interactControl.IsDown())
                {
                    activeInteraction.Interact(referenceObject);
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
