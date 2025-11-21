
using System.Collections;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Play Particle System")]
    public class GameAction_PlayParticleSystem: GameAction
    {
        [SerializeField] private ParticleSystem particleSystem;
        
        public override IEnumerator Execute(GameObject go)
        {
            particleSystem.Play();

            yield return new WaitForSeconds(particleSystem.main.duration);
        }
    }
}