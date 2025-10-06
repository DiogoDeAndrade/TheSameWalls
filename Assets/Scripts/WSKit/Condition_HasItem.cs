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
            var inventory = referenceObject.GetComponent<Inventory>();

            return inventory?.HasItem(item) ?? false;
        }
    }
}