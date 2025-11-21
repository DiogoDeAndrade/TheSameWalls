using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Numeric")]
    public class Condition_Numeric : Condition
    {
        public enum ComparisonOp
        {
            [InspectorName("<")] Less,
            [InspectorName("<=")] LessEqual,
            [InspectorName("==")] Equal,
            [InspectorName("!=")] NotEqual,
            [InspectorName(">")] Greater,
            [InspectorName(">=")] GreaterEqual
        }

        [SerializeField] protected ComparisonOp op = ComparisonOp.GreaterEqual;
        [SerializeReference] protected ValueBase value1 = new ValueLiteral();
        [SerializeReference] protected ValueBase value2 = new ValueLiteral();

        protected override bool EvaluateThis(GameObject referenceObject)
        {
            if (value1 == null || value2 == null) return false;

            float a = value1.Value(referenceObject);
            float b = value2.Value(referenceObject);

            // Tolerance for equality comparisons; tweak if you want this configurable
            const float eps = 1e-5f;

            switch (op)
            {
                case ComparisonOp.Less: return a < b - eps;
                case ComparisonOp.LessEqual: return a <= b + eps;
                case ComparisonOp.Equal: return Mathf.Abs(a - b) <= eps;
                case ComparisonOp.NotEqual: return Mathf.Abs(a - b) > eps;
                case ComparisonOp.Greater: return a > b + eps;
                case ComparisonOp.GreaterEqual: return a >= b - eps;
            }
            return false;
        }
    }
}
