using UnityEngine;
using System.Collections;
using Funk.Data;

namespace Funk.Powerup
{
    public class FreezePowerup : PowerupBase
    {

        protected override void ApplyEffect()
        {
            PlayerState[] allPlayers = _matchContext.PlayersStates;
            for (int i = 0; i < allPlayers.Length; i++)
            {
                if (allPlayers[i] != _affectedPlayerState)
                {
                    allPlayers[i].Speed *= .2f;
                }
            }
        }

        protected override void UnapplyEffect()
        {
            PlayerState[] allPlayers = _matchContext.PlayersStates;
            for (int i = 0; i < allPlayers.Length; i++)
            {
                if (allPlayers[i] != _affectedPlayerState)
                {
                    allPlayers[i].Speed *= 5f;
                }
            }
        }
    }
}
