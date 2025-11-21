using UC;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Is Equipped")]
    public class Condition_IsEquipped : Condition
    {
        [SerializeField] private Hypertag slot;
        [SerializeField] private Item     item;

        protected override bool EvaluateThis(GameObject referenceObject)
        {
            var equipmentManager = GameObject.FindFirstObjectByType<Equipment>();
            if (equipmentManager == null)
            {
                Debug.LogWarning($"Can't find equipment manager on reference object {referenceObject.name}");
                return false;
            }

            if (slot == null) return equipmentManager.IsEquipped(item);

            return equipmentManager.IsEquipped(slot, item);
        }
    }
}