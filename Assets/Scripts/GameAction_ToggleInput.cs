
using UnityEngine;
using UC;
using System.Collections;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Toggle Input")]
    public class GameAction_ToggleInput : GameAction
    {
        public enum State { On, Off, Toggle };

        [SerializeField] private State  state;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            FPSController controller = GameObject.FindFirstObjectByType<FPSController>();
            if (controller != null)
            {
                switch (state)
                {
                    case State.On:
                        controller.enabled = true;
                        break;
                    case State.Off:
                        controller.enabled = false;
                        break;
                    case State.Toggle:
                        controller.enabled = !controller.enabled;
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
    }
}
