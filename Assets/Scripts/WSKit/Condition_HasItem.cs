using UC;
using UnityEngine;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Has Item")]
    public class Condition_HasItem : Condition
    {
        [SerializeField] private Item item;

        protected override bool EvaluateThis(GameObject referenceObject)
        {
            var inventory = GameObject.FindFirstObjectByType<Inventory>();
            if (inventory == null)
            {
                Debug.LogWarning($"Can't find inventory.");
            }

            return inventory?.HasItem(item) ?? false;
        }
    }
}