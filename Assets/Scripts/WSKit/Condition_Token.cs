using UC;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    public class Condition_Token : Condition
    {
        [SerializeField] private Hypertag token;
        protected override bool EvaluateThis(GameObject referenceObject)
        {
            var tokenManager = GameObject.FindFirstObjectByType<TokenManager>();

            return tokenManager?.HasToken(token) ?? false;
        }
    }
}