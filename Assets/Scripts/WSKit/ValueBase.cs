using UC;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    public abstract class ValueBase
    {
        public abstract float Value(GameObject referenceObject);
    }

    [System.Serializable]
    public class ValueLiteral : ValueBase
    {
        [SerializeField] protected float value;

        public override float Value(GameObject referenceObject) => value;
    }

    [System.Serializable]
    public class ValueTokenCount : ValueBase
    {
        [SerializeField] protected Hypertag token;

        public override float Value(GameObject referenceObject)
        {
            var tokenManager = GameObject.FindFirstObjectByType<TokenManager>();
            return tokenManager.GetTokenCount(token);
        }
    }
}
