using Funk.Data;
using UnityEngine;

namespace Funk
{
    public class Main : MonoBehaviour
    {
        private const int PLAYER_LIFE = 10;

        private Match _match;
        private MatchInput _input;
        private MatchPlayback _playback;

        private void Awake()
        {
            var mapData = new MapData
            {
                Name = "DefaultMap"
            };

            var playerData = new PlayerData[2]
            {
                new PlayerData() {
                    Index = 0,
                    Name = "Player1",
                    Train = "RedTrain",
                    Life = PLAYER_LIFE,
                    InputMap = InputMap.KeyboardWASD },

                new PlayerData() {
                    Index = 1,
                    Name = "Player2",
                    Train = "BlueTrain",
                    Life = PLAYER_LIFE,
                    InputMap = InputMap.KeyboardArrows }
            };

            _match = new Match(mapData, playerData);
            _input = new MatchInput(_match);
            _playback = new MatchPlayback(_match);
        }

        private void Update()
        {
            var actions = _input.GetActions();
            _playback.Play(actions);
        }
    }
}