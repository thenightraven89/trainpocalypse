using Funk.Data;
using Funk.Powerup;
using UnityEngine;

namespace Funk
{
    public class Main : MonoBehaviour
    {
        private const int PLAYER_LIFE = 10;

        private Match _match;
        private MatchPlayback _playback;

        private void Awake()
        {
            var mapSettings = new MapSettings
            {
                Name = "DefaultMap",
                Width = 40,
                Height = 40
            };

            var playerSettings = new PlayerSettings[]
            {
                new PlayerSettings() {
                    Index = 0,
                    Name = "Player1",
                    Train = "RedTrain",
                    Block = "RedBlock",
                    InputMap = InputMap.KeyboardWASD }
                ,

                new PlayerSettings() {
                    Index = 1,
                    Name = "Player2",
                    Train = "BlueTrain",
                    Block = "BlueBlock",
                    InputMap = InputMap.KeyboardArrows }
            };

            var matchSettings = new MatchSettings(10, 4, 8, new PowerupSettings[] {
                new PowerupSettings(typeof(ClockPowerup), 3, 0f, 1f),
                new PowerupSettings(typeof(FreezePowerup), 1, 10f, 0.1f),
                new PowerupSettings(typeof(InvulnerabilityPowerup), 1, 10, 0.4f),
                new PowerupSettings(typeof(ExtraLifePowerup), 1, 30, 0.3f)});

            _match = new Match(mapSettings, playerSettings, matchSettings);
            _playback = new MatchPlayback(_match);
        }

        private void Update()
        {
            _playback.Update();
        }
    }
}