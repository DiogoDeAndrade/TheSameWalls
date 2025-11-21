
using System.Collections;
using UnityEngine;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Wait")]
    public class GameAction_Wait : GameAction
    {
        [SerializeField] private float      waitTime;

        public override IEnumerator Execute(GameObject go)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }
}
