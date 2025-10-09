
using System.Collections;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    public class GameAction_Wait : GameAction
    {
        [SerializeField] private float      waitTime;

        public override IEnumerator Execute(GameObject go)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }
}
