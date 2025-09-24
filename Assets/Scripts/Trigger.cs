using UnityEngine;

[System.Serializable]
public abstract class Trigger
{
    public abstract bool ShouldTrigger(Trigger trigger, GameObject go);

    protected bool          _alreadyTriggered = false;
    protected ActManager    _actManager;

    public void SetTriggered()
    {
        _alreadyTriggered = true;
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
}
