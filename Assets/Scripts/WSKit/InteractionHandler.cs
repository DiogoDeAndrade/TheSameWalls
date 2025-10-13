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
        [SerializeField] 
        private CursorDef           inputDisableCursor;
        [SerializeField]
        private bool               debugMode;

        CursorManager cursorManager;
        FPSController inputController;
        GameObject    referenceObject => masterInteractionObject ? masterInteractionObject : gameObject;

        private void Start()
        {
            inputController = GetComponentInParent<FPSController>();
            cursorManager = FindFirstObjectByType<CursorManager>();
            interactControl.playerInput = playerInput;
        }

        private void Update()
        {
            if (!interactionEnabled) return;

            var hits = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, interactionRange, interactionLayers, QueryTriggerInteraction.Collide);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            if (debugMode)
            {
                Debug.Log($"Spherecast hits = {hits.Length}");
                foreach (var hit in hits)
                {
                    Debug.Log($"Hit {hit.collider.name} at {hit.distance}");
                }
            }

            Interactable activeInteraction = null;

            foreach (var hit in hits)
            {
                var localInteractables = hit.collider.GetComponents<Interactable>();
                if (localInteractables.Length > 0)
                {
                    if (debugMode) Debug.Log($"Object {hit.collider.name} has {localInteractables.Length} interactables...");

                    Array.Sort(localInteractables, (a, b) => b.priority.CompareTo(a.priority));

                    // Check if any is interactable
                    foreach (var i in localInteractables)
                    {
                        if (i.CanInteract(referenceObject))
                        {
                            activeInteraction = i;
                            break;
                        }
                    }
                    if (activeInteraction == null)
                    {
                        if (debugMode) Debug.Log($"No interactable found on {hit.collider.name}");
                    }
                }
                if (activeInteraction) break;
            }

            if (activeInteraction)
            {
                if (debugMode) Debug.Log($"Interactable {activeInteraction.name}  is interactable");

                // Handle cursor
                if (cursorManager)
                {
                    if (inputController.enabled)
                        cursorManager.SetCursor(activeInteraction.cursor);
                    else
                        cursorManager.SetCursor(inputDisableCursor);
                }

                // Interact
                if (interactControl.IsDown())
                {
                    if (debugMode) Debug.Log($"Interacting with {activeInteraction.name}...");

                    activeInteraction.Interact(referenceObject, this);
                }
            }
            else
            {
                if (cursorManager)
                {
                    if (inputController.enabled)
                        cursorManager.SetCursor(null);
                    else
                        cursorManager.SetCursor(inputDisableCursor);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);

            if (debugMode)
            {
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);
            }
        }
    }
}
