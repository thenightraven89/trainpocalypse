using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funk.Data
{
    public class MatchState
    {
        public int PlayersAlive
        {
            get
            {
                int playersAlive = 0;
                foreach (var key in _playersStates.Keys)
                {
                    if (_playersStates[key].IsDead)
                    {
                        playersAlive++;
                    }
                }
                return playersAlive;
            }
        }

        public bool MatchOver
        {
            get { return PlayersAlive > 1; }
        }

        public PlayerState[] PlayerStates { get { return _playersStates.Values.ToArray(); } }

        private Dictionary<string, PlayerState> _playersStates;

        public MatchState(Dictionary<string, PlayerState> playerStates)
        {
            _playersStates = playerStates;
        }

        public PlayerState GetPlayerState(string player)
        {
            if (_playersStates.ContainsKey(player))
                return _playersStates[player];
            return null;
        }
    }
}
