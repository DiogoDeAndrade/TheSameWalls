
using UnityEngine;
using UC;
using UC.Interaction;
using System.Collections;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Change Exposure")]
    public class GameAction_ChangeExposure: GameAction
    {
        [SerializeField] private float exposure = 4.0f;
        [SerializeField] private float duration = 0.0f;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            Camera camera = GameObject.FindFirstObjectByType<Camera>();
            if (camera != null)
            {
                var ctrl = camera.GetComponent<ColorAdjustmentCtrl>();

                if (duration == 0.0f)
                {
                    ctrl.SetPostExposure(exposure);
                }
                else
                {
                    float currentExposure = ctrl.GetPostExposure();

                    float elapsed = 0.0f;
                    while (elapsed < duration)
                    {
                        elapsed += Time.deltaTime;
                        float t = Mathf.Clamp01(elapsed / duration);
                        float newExposure = Mathf.Lerp(currentExposure, exposure, t);
                        ctrl.SetPostExposure(newExposure);
                        yield return null;
                    }

                    ctrl.SetPostExposure(exposure);
                }
            }
            yield return null;
        }
    }
}
