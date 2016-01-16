using Funk.Data;
using Funk.Powerup;
using UnityEngine;

namespace Funk
{
    public class Main : MonoBehaviour
    {
        private const int PLAYER_LIFE = 10;

        private Match _match;
        private MatchInput _input;
        private MatchPlayback _playback;
        private PowerupSpawner _powerupSpawner;
        private PowerupController _powerupController;

        private void Awake()
        {
            var mapData = new MapData
            {
                Name = "DefaultMap",
                Width = 40,
                Height = 40
            };

            var playerData = new PlayerData[2]
            {
                new PlayerData() {
                    Index = 0,
                    Name = "Player1",
                    Train = "RedTrain",
                    Block = "RedBlock",
                    Life = PLAYER_LIFE,
                    InputMap = InputMap.KeyboardWASD },

                new PlayerData() {
                    Index = 1,
                    Name = "Player2",
                    Train = "BlueTrain",
                    Block = "BlueBlock",
                    Life = PLAYER_LIFE,
                    InputMap = InputMap.KeyboardArrows }
            };

            var matchSettings = new MatchSettings(10, 5,
                new PowerupSettings[] { new PowerupSettings(typeof(PowerupBase), 3, 0f, 1f) }
                );

            _match = new Match(mapData, playerData, matchSettings);
            _input = new MatchInput(_match);
            _playback = new MatchPlayback(_match);
            _powerupSpawner = GetComponent<PowerupSpawner>();
            _powerupSpawner.LoadPowerups(_match.MatchSettings.PowerupsAvailable);
            _powerupController = new PowerupController(_match, _powerupSpawner);
        }

        private void Update()
        {
            var actions = _input.GetActions();
            _playback.Play(actions);
            _powerupController.Run(Time.deltaTime);
        }
    }
}