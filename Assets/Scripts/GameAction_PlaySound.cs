
using UnityEngine;
using UC;

[System.Serializable]
public class GameAction_PlaySound : GameAction
{
    [SerializeField] private SoundDef   sound;

    public override void Execute(GameObject go)
    {
        sound.Play();
    }
}
