
using System.Collections;
using UnityEngine;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Toggle Object")]
    public class GameAction_ToggleObject : GameAction
    {
        public enum State { Enable, Disable, Toggle };

        [SerializeField] private GameObject targetGameObject;
        [SerializeField] private State      state = State.Disable;

        public override IEnumerator Execute(GameObject go)
        {
            switch (state)
            {
                case State.Enable:
                    targetGameObject.SetActive(true);
                    break;
                case State.Disable:
                    targetGameObject.SetActive(false);
                    break;
                case State.Toggle:
                    targetGameObject.SetActive(!targetGameObject.activeSelf);
                    break;
                default:
                    break;
            }

            yield return null;
        }
    }
}