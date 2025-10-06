
using UnityEngine;

namespace WSKit
{
    [System.Serializable]
    public abstract class GameAction

    {
        public abstract void Execute(GameObject go);
    }
}