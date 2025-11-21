
using UnityEngine;
using UC;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Change Scene")]
    public class GameAction_ChangeScene : GameAction
    {
        [SerializeField, Scene] private string  scene;
        [SerializeField, Min(0f)] private float   fadeTime = 1.0f;
        [SerializeField] private Color   fadeColor = Color.black;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            FullscreenFader.FadeOut(fadeTime, fadeColor, () =>
            {
                SceneManager.LoadScene(scene);
            });

            yield return null;
        }
    }
}
