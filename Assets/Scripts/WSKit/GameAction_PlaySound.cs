
using UnityEngine;
using UC;
using System.Collections;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Play Sound")]
    public class GameAction_PlaySound : GameAction
    {
        [SerializeField] private SoundDef sound;

        protected override void SetDefaultValues()
        {
            wait = true; 
        }

        public override IEnumerator Execute(GameObject go)
        {
            var snd = sound.Play();

            if (wait)
            { 
                while (snd.isPlaying)
                {
                    yield return null;
                }
            }
        }
    }
}