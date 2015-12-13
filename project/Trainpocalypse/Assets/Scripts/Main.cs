﻿using Funk.Data;
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
            var data = new PlayerData[2]
            {
                new PlayerData() {
                    Name = "Player1",
                    Train = "BlueTrain",
                    Life = PLAYER_LIFE,
                    SelectedInputMap = InputMap.KeyboardWASD },

                new PlayerData() {
                    Name = "Player2",
                    Train = "RedTrain",
                    Life = PLAYER_LIFE,
                    SelectedInputMap = InputMap.KeyboardArrows }
            };

            _match = new Match(data);
            _input = new MatchInput(data);
            _playback = new MatchPlayback(data);
        }

        private void Update()
        {
            var actions = _input.GetActions();
            _playback.Play(actions);
        }
    }
}