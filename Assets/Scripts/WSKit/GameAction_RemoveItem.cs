
using UnityEngine;
using UC;
using System.Collections;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Remove Item")]
    public class GameAction_RemoveItem : GameAction
    {
        [SerializeField] private Item   item;
        [SerializeField] private int    quantity = 1;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            if (item == null)
            {
                Debug.LogWarning("No item specified to remove.");
            }
            var inventory = GameObject.FindFirstObjectByType<Inventory>();
            if (inventory == null)
            {
                Debug.LogWarning($"Can't find inventory.");
            }
            inventory?.Remove(item, quantity);

            yield return null;
        }
    }
}
