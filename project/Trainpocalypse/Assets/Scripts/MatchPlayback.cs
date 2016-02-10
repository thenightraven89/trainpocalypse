using Funk.Collision;
using Funk.Data;
using Funk.Powerup;
using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
    public class MatchPlayback
    {
        private const string PATH_BLOCKS = "Blocks/";
        private const string PATH_TRAINS = "Trains/";
        private const string PATH_MAPS = "Maps/";
        private const string PATH_POWERUPS = "Powerups";

        private Dictionary<string, Train> _trains;
        private Dictionary<string, Transform> _spawnPoints;
        private CollisionController _collisionCenter;
        private PowerupHandler _powerupHandler;

        private MatchState _matchState;

        public MatchPlayback(Match match)
        {
            _spawnPoints = new Dictionary<string, Transform>();
            _trains = new Dictionary<string, Train>();
            _collisionCenter = new CollisionController();

            var mapPath = string.Format("{0}{1}", PATH_MAPS, match.MapSettings.Name);
            var mapSource = Resources.Load(mapPath) as GameObject;
            var mapObject = GameObject.Instantiate(mapSource);
            var playerStates = new Dictionary<string, PlayerState>();

            foreach (var player in match.PlayerSettings)
            {
                _spawnPoints.Add(player.Name,
                    mapObject.transform.Find("Spawn" + player.Index.ToString()));

                var trainPath = string.Format("{0}{1}", PATH_TRAINS, player.Train);
                var trainSource = Resources.Load(trainPath) as GameObject;
                var trainObject = GameObject.Instantiate(
                    trainSource,
                    _spawnPoints[player.Name].position,
                    _spawnPoints[player.Name].rotation
                    ) as GameObject;

                var trainComponent = trainObject.GetComponent<Train>();
                var trainState = new PlayerState(match.MatchSettings.MaxLives, 
                    match.MatchSettings.DefaultPlayerSpeed);
                //trainComponent.TrainState = trainState;
                trainComponent.SubscribeToCollision(HandleCollision);
                trainComponent.TrainName = player.Name;
                trainComponent.Speed = trainState.Speed;
                trainState.SubscribeToModelChanged(trainComponent.SetSpeed);
                playerStates.Add(player.Name, trainState);
                _trains.Add(player.Name, trainComponent);
            }
            
            _powerupHandler = new PowerupHandler(match, PATH_POWERUPS);
            _matchState = new MatchState(playerStates);

            _collisionCenter.AddCollisionHandler(new PowerupCollisionHandler(_powerupHandler));
            _collisionCenter.AddCollisionHandler(new ObstacleCollisionHandler(Respawn));

            match.Start(_matchState);
        }

        public void Respawn(Train train)
        {
            train.Explode();

            train.Reset(_spawnPoints[train.TrainName].position,
                _spawnPoints[train.TrainName].rotation);
        }

        public void Play(List<PlayerAction> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                _trains[actions[i].PlayerName].Play(actions[i].Type);
            }
        }

        public void RunPowerUpSpawner(float deltaTime)
        {
            _powerupHandler.Update(deltaTime);
        }

        private void HandleCollision(object sender, CollisionEventArgs args)
        {
            args.MatchState = _matchState;
            _collisionCenter.HandleCollision(sender, args);
        }
    }
}