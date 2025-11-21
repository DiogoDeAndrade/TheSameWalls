using UC;
using UnityEngine;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Has Token")]
    public class Condition_HasToken : Condition
    {
        [SerializeField] private Hypertag token;
        protected override bool EvaluateThis(GameObject referenceObject)
        {
            var tokenManager = GameObject.FindFirstObjectByType<TokenManager>();
            if (tokenManager == null)
            {
                Debug.LogWarning($"Can't find token manager on reference object {referenceObject.name}");
                return false;
            }

            return tokenManager.HasToken(token);
        }
    }
}