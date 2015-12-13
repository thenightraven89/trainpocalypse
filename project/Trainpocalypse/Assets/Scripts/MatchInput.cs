using Funk.Data;
using Funk.Player;
using System.Collections.Generic;

namespace Funk
{
    public class MatchInput
    {
        private List<IPlayer> _players;
        private List<Action> _actions;

        public MatchInput(PlayerData[] playerData)
        {
            _players = new List<IPlayer>();
            _actions = new List<Action>();
            
            foreach (var player in playerData)
            {
                _players.Add(new LocalPlayer(player));
            }
        }

        public List<Action> GetActions()
        {
            _actions.Clear();

            for (int i = 0; i < _players.Count; i++)
            {
                var action = _players[i].GetAction();
                UnityEngine.Debug.Log(action);
                _actions.Add(action);
            }

            return _actions;
        }
    }
}