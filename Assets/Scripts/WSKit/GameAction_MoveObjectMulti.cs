
using System.Collections;
using UC;
using UnityEngine;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Move Object (Multiple)")]
    public class GameAction_MoveObjectMulti : GameAction
    {
        [SerializeField] 
        private Transform   objectToMove;
        [SerializeField] 
        private Transform[] targetTransforms;
        [SerializeField, Min(0.0f)] 
        private float       duration = 0.0f;

        public override IEnumerator Execute(GameObject go)
        {
            var index = GetCurrentTransformIndex();
            if (index == -1)
            {
                index = 1;
            }
            else
            {
                index = (index + 1) % targetTransforms.Length;
            }

            var targetTransform = targetTransforms[index];

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
                objectToMove.localScale = targetTransform.localScale;
                yield return null;
            }
        }

        int GetCurrentTransformIndex()
        {
            float minDist = float.MaxValue;
            float minDistR = float.MaxValue;
            int   ret = -1;
            for (int i = 0; i < targetTransforms.Length; i++) 
            {
                float d = Vector3.Distance(targetTransforms[i].position, objectToMove.position);
                if (d  < minDist)
                {
                    float dr = Quaternion.Angle(targetTransforms[i].rotation, objectToMove.rotation);
                    if ((dr < minDistR) || (d < minDist))
                    {
                        minDist = d;
                        minDistR = dr;
                        ret = i;
                    }
                }
            }

            return ret;
        }
    }
}