using UnityEngine;
using WSKit;

[System.Serializable]
public class Condition_TimeFromStart : Condition
{
    [SerializeField] private float elapsedTime = 1.0f;

    protected override bool EvaluateThis(GameObject referenceObject)
    {
        if (_alreadyTriggered) return false;

        return (actManager.timeFromStart > elapsedTime);
    }

    public ActManager actManager
    {
        get
        {
            if (_actManager) return _actManager;

            _actManager = GameObject.FindFirstObjectByType<ActManager>();

            return _actManager;
        }
    }
    protected ActManager _actManager;
}
