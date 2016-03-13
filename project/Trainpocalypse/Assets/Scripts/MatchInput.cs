using Funk.Data;
using Funk.Player;
using System.Collections.Generic;

namespace Funk
{
    public class MatchInput
    {
        private List<IPlayer> _players;
        private List<PlayerAction> _actions;

        public MatchInput(Match match)
        {
            _players = new List<IPlayer>();
            _actions = new List<PlayerAction>();
            
            foreach (var player in match.PlayerSettings)
            {
                _players.Add(new LocalPlayer(player));
            }
        }

        public List<PlayerAction> GetActions()
        {
            _actions.Clear();

            for (int i = 0; i < _players.Count; i++)
            {
                UnityEngine.Debug.Log(_players[i]);
                var action = _players[i].GetAction();
                _actions.Add(action);
            }

            return _actions;
        }
    }
}