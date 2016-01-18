using UnityEngine;
using System.Collections;
using Funk.Data;

namespace Funk.Powerup
{
    public class ExtraLifePowerup : PowerupBase
    {

        protected override void ApplyEffect()
        {
            _affectedPlayerState.Lives += 1;
        }

        protected override void UnapplyEffect()
        {
            //NADA
        }
    }
}
