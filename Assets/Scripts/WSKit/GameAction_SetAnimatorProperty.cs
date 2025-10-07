
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace WSKit
{
    [System.Serializable]
    public class GameAction_SetAnimatorProperty : GameAction
    {
        [SerializeField] private Animator animator;
        [SerializeField, AnimatorParam(nameof(animator))] private string propName;
        [SerializeField] private bool bValue;
        [SerializeField] private float fValue;
        [SerializeField] private int iValue;

        public AnimatorControllerParameterType? propType
        {
            get
            {
                if (animator == null) return null;
                var propHash = Animator.StringToHash(propName);
                if (!animator.GetParameterByHash(propHash, out var parameterType))
                    return null;

                return parameterType;
            }
        }

        public override IEnumerator Execute(GameObject go)
        {
            if (animator == null)
            {
                Debug.LogWarning($"Animator not setup on action {go.name}!");
                yield return null;
            }
            else
            {
                var propHash = Animator.StringToHash(propName);
                if (!animator.GetParameterByHash(propHash, out var parameterType))
                {
                    Debug.LogWarning($"Parameter {propName} not setup on animator in {animator.name}!");
                    yield return null;
                }
                else
                {
                    switch (parameterType)
                    {
                        case AnimatorControllerParameterType.Float:
                            animator.SetFloat(propName, fValue);
                            break;
                        case AnimatorControllerParameterType.Int:
                            animator.SetInteger(propName, iValue);
                            break;
                        case AnimatorControllerParameterType.Bool:
                            animator.SetBool(propName, bValue);
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            animator.SetTrigger(propName);
                            break;
                        default:
                            break;
                    }

                    yield return null;
                }
            }
        }
    }
}