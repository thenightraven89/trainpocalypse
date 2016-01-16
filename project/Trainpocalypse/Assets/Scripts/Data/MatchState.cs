using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funk.Data
{
    public class MatchState
    {
        public PlayerState[] PlayersStates { get; private set; }
        public int PlayersAlive
        {
            get
            {
                int plAlive = 0;
                for (int i = 0; i < PlayersStates.Length; i++)
                {
                    if (!PlayersStates[i].IsDead)
                    {
                        plAlive++;
                    }
                }
                return plAlive;
            }
        }

        public bool MatchOver
        {
            get { return PlayersAlive > 1; }
        }

        public MatchState(PlayerState[] playerStates)
        {
            PlayersStates = playerStates;
        }

    }
}
