using UC;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
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