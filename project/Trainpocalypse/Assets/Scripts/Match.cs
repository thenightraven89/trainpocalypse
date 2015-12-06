using Funk.Data;
using Funk.Player;

namespace Funk
{
    public class Match
    {
        private MatchData _matchData;
        private IPlayer[] _players;

        public Match(PlayerData[] playerData)
        {
            var playerCount = playerData.Length;

            _players = new IPlayer[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                _players[i] = new LocalPlayer(playerData[i]);
            }

            _matchData = new MatchData();
            _matchData.PlayerLife = new int[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                _matchData.PlayerLife[i] = 0;
            }
        }

        public MatchData Update()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].Update();
            }

            return _matchData;
        }
    }
}