
using System.Collections;
using UC;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    public class GameAction_MoveObject : GameAction
    {
        [SerializeField] 
        private Transform   objectToMove;
        [SerializeField] 
        private Transform   targetTransform;
        [SerializeField, Min(0.0f)] 
        private float       duration = 0.0f;

        public override IEnumerator Execute(GameObject go)
        {
            if (duration > 0.0f)
            {
                objectToMove.MoveTo(targetTransform.position, duration);
                objectToMove.RotateTo(targetTransform.rotation, duration);
                objectToMove.LocalScaleTo(targetTransform.localScale, duration);
                yield return new WaitForSeconds(duration);
            }
            else
            {
                objectToMove.position = targetTransform.position;
                objectToMove.rotation = targetTransform.rotation;
                objectToMove.localScale= targetTransform.localScale;
                yield return null;
            }
        }
    }
}