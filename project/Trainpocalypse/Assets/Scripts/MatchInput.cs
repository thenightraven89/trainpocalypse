using Funk.Data;
using Funk.Player;
using System.Collections.Generic;

namespace Funk
{
    public class MatchInput
    {
        private IPlayer[] _players;
        private List<Action> _actions;

        public MatchInput(PlayerData[] playerData)
        {
            _actions = new List<Action>();

            _players = new IPlayer[playerData.Length];
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new LocalPlayer(playerData[i]);
            }
        }

        public List<Action> GetActions()
        {
            _actions.Clear();

            for (int i = 0; i < _players.Length; i++)
            {
                var action = _players[i].GetAction();
                UnityEngine.Debug.Log(action);
                _actions.Add(action);
            }

            return _actions;
        }
    }
}