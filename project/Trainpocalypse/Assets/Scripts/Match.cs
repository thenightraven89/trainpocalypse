using Funk.Data;
using Funk.Player;

namespace Funk
{
    public class Match
    {
        private MatchData _state;
        private IPlayer[] _players;

        public Match(PlayerData[] data)
        {
            var playerCount = data.Length;

            _players = new IPlayer[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                _players[i] = new LocalPlayer(data[i]);
            }


            _state = new MatchData();
        }

        public MatchData Update()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].Update();
            }

            return _state;
        }
    }
}