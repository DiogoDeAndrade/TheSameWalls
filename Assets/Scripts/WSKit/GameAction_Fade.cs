
using System.Collections;
using UC;
using UnityEngine;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Fade")]
    public class GameAction_Fade : GameAction
    {
        public enum State { In, Out };

        [SerializeField] private State      state = State.Out;
        [SerializeField] private float      fadeDuration = 1.0f;
        [SerializeField] private Color      fadeColor = Color.black;

        public override IEnumerator Execute(GameObject go)
        {
            switch (state)
            {
                case State.Out:
                    FullscreenFader.FadeOut(fadeDuration, fadeColor);
                    break;
                case State.In:
                    FullscreenFader.FadeIn(fadeDuration, fadeColor);
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(fadeDuration);
        }
    }
}
