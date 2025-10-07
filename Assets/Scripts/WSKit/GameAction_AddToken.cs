
using UnityEngine;
using UC;
using System.Collections;

namespace WSKit
{

    [System.Serializable]
    public class GameAction_AddToken : GameAction
    {
        [SerializeField] private Hypertag token;
        [SerializeField] private int      quantity = 1;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            if (token == null)
            {
                Debug.LogWarning("No token specified to add.");
            }
            var tokenManager = GameObject.FindFirstObjectByType<TokenManager>();
            if (tokenManager == null)
            {
                Debug.LogWarning("Can't find token manager.");
            }
            tokenManager?.Add(token, quantity);

            yield return null;
        }
    }
}
