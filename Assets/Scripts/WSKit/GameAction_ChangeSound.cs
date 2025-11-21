using System.Collections;
using UC;
using UnityEngine;
using UC.Interaction;

[System.Serializable]
[GameActionName("WSL/Change Sound")]
public class GameAction_ChangeSound : GameAction
{
    public enum Property { Volume, Pitch, Disable };

    [SerializeField] private Hypertag soundTag;
    [SerializeField] private Property property;
    [SerializeField] private float    targetValue = 1.0f;
    [SerializeField] private float    duration = 1.0f;
    [SerializeField] private bool     stopOnEnd = false;

    public override IEnumerator Execute(GameObject go)
    {
        var sound = SoundManager.GetSound(soundTag);
        if (sound == null)
        {
            var n = (soundTag) ? (soundTag.name) : "UNKNOWN";
            Debug.LogWarning($"Can't find sound tagged with {n}!");
            yield break;
        }

        Tweener.BaseInterpolator interpolator = null;

        switch (property)
        {
            case Property.Volume:
                interpolator = sound.FadeTo(targetValue, duration);
                break;
            case Property.Pitch:
                interpolator = sound.PitchTo(targetValue, duration);
                break;
            case Property.Disable:
                sound.Stop();
                yield return null;
                break;
            default:
                break;
        }

        if ((interpolator != null) && (stopOnEnd))
        {
            interpolator.Done(() =>
            {
                sound.Stop();
            });
        }

        yield return new WaitForSeconds(duration);
    }
}
