using UnityEngine;

namespace Funk.Powerup
{
    public class ClockPowerup : PowerupBase
    {
        [SerializeField]
        private float _delayAddition = .2f;

        protected override void ApplyEffect()
        {
            _affectedPlayerState.DominoDelay += _delayAddition;
        }

        protected override void UnapplyEffect()
        {
            _affectedPlayerState.DominoDelay -= _delayAddition;
        }
    }
}
