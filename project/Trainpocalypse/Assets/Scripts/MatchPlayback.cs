using Funk.Collision;
using Funk.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
    public class MatchPlayback
    {
        private const string PATH_BLOCKS = "Blocks/";
        private const string PATH_TRAINS = "Trains/";
        private const string PATH_MAPS = "Maps/";

        private Dictionary<string, Train> _trains;
        private List<Transform> _spawnPoints;
        private CollisionController _collisionCenter;

        public MatchPlayback(Match match)
        {
            _spawnPoints = new List<Transform>();
            _trains = new Dictionary<string, Train>();
            _collisionCenter = new CollisionController(match,
               new List<ICollisionHandler> { new PowerupCollisionHandler() });

            var mapPath = string.Format("{0}{1}", PATH_MAPS, match.MapData.Name);
            var mapSource = Resources.Load(mapPath) as GameObject;
            var mapObject = GameObject.Instantiate(mapSource);
            var playerStates = new List<PlayerState>();

            foreach (var player in match.PlayerData)
            {
                _spawnPoints.Add(mapObject.transform.Find("Spawn" + player.Index.ToString()));

                var trainPath = string.Format("{0}{1}", PATH_TRAINS, player.Train);
                var trainSource = Resources.Load(trainPath) as GameObject;
                var trainObject = GameObject.Instantiate(
                    trainSource,
                    _spawnPoints[player.Index].position,
                    _spawnPoints[player.Index].rotation
                    ) as GameObject;

                var trainComponent = trainObject.GetComponent<Train>();
                var trainState = new PlayerState(trainComponent,
                    match.MatchSettings.MaxLives, match.MatchSettings.DefaultPlayerSpeed);
                trainComponent.TrainState = trainState;
                trainComponent.SubscribeToCollision(_collisionCenter.HandleCollision);
                playerStates.Add(trainState);
                _trains.Add(player.Name, trainComponent);
            }

            var matchState = new MatchState(playerStates.ToArray());
            match.Start(matchState);
           
        }

        public void Play(List<PlayerAction> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                _trains[actions[i].PlayerName].Play(actions[i].Type);
            }
        }
    }
}