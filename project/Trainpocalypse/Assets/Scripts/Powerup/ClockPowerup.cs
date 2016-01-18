using UnityEngine;
using System.Collections;
using Funk.Data;

namespace Funk.Powerup
{
    public class ClockPowerup : PowerupBase
    {

        protected override void ApplyEffect()
        {
            _affectedPlayerState.DominoDelay += .2f;
        }

        protected override void UnapplyEffect()
        {
            _affectedPlayerState.DominoDelay -= .2f;
        }
    }
}
