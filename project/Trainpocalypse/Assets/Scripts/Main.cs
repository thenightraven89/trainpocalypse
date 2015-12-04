using Funk.Data;
using UnityEngine;

namespace Funk
{
    public class Main : MonoBehaviour
    {
        private Match _match;
        private MatchVisuals _visuals;

        private void Awake()
        {
            var data = new PlayerData[2]
            {
                new PlayerData() { Name = "Player1", Model = "BlueTrain" },
                new PlayerData() { Name = "Player2", Model = "RedTrain" }
            };

            _match = new Match(data);
            _visuals = new MatchVisuals(data);
        }

        private void Update()
        {
            var state = _match.Update();
            _visuals.Update(state);
        }
    }
}