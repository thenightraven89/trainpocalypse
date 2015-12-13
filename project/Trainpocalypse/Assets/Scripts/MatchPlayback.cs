using Funk.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
    public class MatchPlayback
    {
        private const string PATH_TRAINS = "/Trains/";

        private Dictionary<string, Train> _trains;

        public MatchPlayback(PlayerData[] playerData)
        {
            _trains = new Dictionary<string, Train>();

            foreach (var player in playerData)
            {
                var trainPath = string.Format("{0}{1}", PATH_TRAINS, player.Train);
                var trainSource = Resources.Load(trainPath) as GameObject;
                var trainObject = GameObject.Instantiate(trainSource);
                var trainComponent = trainObject.GetComponent<Train>();
                _trains.Add(player.Name, trainComponent);
            }
        }

        public void Play(List<Action> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                _trains[actions[i].PlayerName].Play(actions[i].Type);
            }
        }
    }
}