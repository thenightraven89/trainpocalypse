using Funk.Collision;
using Funk.Data;
using Funk.Powerup;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Funk
{
    public class MatchPlayback
    {
        [SerializeField]
        private Text _info;

        private const string PATH_BLOCKS = "Blocks/";
        private const string PATH_TRAINS = "Trains/";
        private const string PATH_MAPS = "Maps/";
        private const string PATH_POWERUPS = "Powerups";

        private Dictionary<string, Train> _trains;
        private Dictionary<string, Transform> _spawnPoints;
        //private CollisionController _collisionCenter;
        //private PowerupHandler _powerupHandler;

        //private MatchState _matchState;
        private Match _match;
        //private GameObject _mapObject;

        private Dictionary<Type, Action<IPlaybackAction>> _handlers;

        private List<IPlaybackAction> _actions;

        private Map _map;

        public MatchPlayback(Match match)
        {
            _match = match;
            _actions = new List<IPlaybackAction>();
            _trains = new Dictionary<string, Train>();

            _handlers = new Dictionary<Type, Action<IPlaybackAction>>()
            {
                { typeof(TrainSpawnAction),  HandleTrainSpawn },
                { typeof(TrainExplodeAction), HandleTrainExplode },
                { typeof(TrainSteerAction), HandleTrainSteer },
                { typeof(PowerupSpawnAction), HandlePowerupSpawn},
                { typeof(MapLoadAction), HandleMapLoad }
            };

            Play(new MapLoadAction(_match.MapSettings));

            foreach (var settings in match.PlayerSettings)
            {
                Play(new TrainSpawnAction(settings));
            }
        }

        private void HandleMapLoad(IPlaybackAction obj)
        {
            var action = obj as MapLoadAction;
            var settings = action.Settings;

            var mapPrefab = Resources.Load(PATH_MAPS + settings.Name) as GameObject;
            var mapObject = GameObject.Instantiate(mapPrefab);
            _map = mapObject.GetComponent<Map>();
            _spawnPoints = new Dictionary<string, Transform>();

            for (int i = 0; i < _match.PlayerSettings.Length; i++)
            {
                _spawnPoints.Add(
                    _match.PlayerSettings[i].Name,
                    _map.SpawnPoints[i]);
            }
        }

        private void HandlePowerupSpawn(IPlaybackAction obj)
        {
            var action = obj as PowerupSpawnAction;
            var powerUpPrefab = action.PowerUp;
            var position = action.Position;
            GameObject.Instantiate(
                powerUpPrefab,
                position,
                Quaternion.identity);
        }

        private void HandleTrainSteer(IPlaybackAction obj)
        {
            var action = obj as TrainSteerAction;
            var train = _trains[action.TrainId];
            var position = action.Position;
            var direction = action.Direction;
            train.Steer(position, direction);
        }

        private void HandleTrainExplode(IPlaybackAction obj)
        {
            var action = obj as TrainExplodeAction;
            var trainId = action.TrainId;

            GameObject.Destroy(_trains[trainId]);
            _trains.Remove(trainId);
        }

        private void HandleTrainSpawn(IPlaybackAction obj)
        {
            var action = obj as TrainSpawnAction;
            var settings = action.Settings;

            var trainPrefab = Resources.Load(PATH_TRAINS + settings.Train);
            var spawnPoint = _spawnPoints[settings.Name];

            var trainObject = GameObject.Instantiate(
                trainPrefab,
                spawnPoint.position,
                spawnPoint.rotation) as GameObject;

            var trainState = new PlayerState(
                settings.Name,
                _match.MatchSettings.MaxLives,
                _match.MatchSettings.DefaultPlayerSpeed);

            var train = trainObject.GetComponent<Train>();
            train.TrainName = trainState.TrainName;
            train.Speed = trainState.Speed;

            train.AssignInputMap(settings.InputMap);

            _trains.Add(train.TrainName, train);
        }

        public void Update()
        {
            _actions.Clear();

            foreach (var train in _trains.Values)
            {
                var trainActions = train.GetActions();

                if (trainActions != null)
                {
                    _actions.AddRange(trainActions);
                }

                train.ClearActions();
            }

            Play(_actions);
        }

        public void Play(IPlaybackAction action)
        {
            //Debug.Log("playing " + action.ToString());
            _handlers[action.GetType()](action);
        }

        public void Play(ICollection<IPlaybackAction> actions)
        {
            foreach (var action in actions)
            {
                Play(action);
            }
        }

        //public override void OnServerAddPlayer(NetworkConnection conn,
        //    short playerControllerId)
        //{
        //    Debug.Log(".|.");
        //    PlayerSettings player = _match.PlayerSettings.First(p => p.Index == playerControllerId);
        //    _spawnPoints.Add(player.Name,
        //           _mapObject.transform.Find("Spawn" + player.Index.ToString()));

        //    Debug.Log(playerControllerId);

        //    var trainPath = string.Format("{0}{1}", PATH_TRAINS, player.Train);
        //    var trainSource = Resources.Load(trainPath) as GameObject;
        //    var trainObject = GameObject.Instantiate(
        //        trainSource,
        //        _spawnPoints[player.Name].position,
        //        _spawnPoints[player.Name].rotation
        //        ) as GameObject;

        //    var trainComponent = trainObject.GetComponent<Train>();
        //    var trainState = new PlayerState(player.Name, _match.MatchSettings.MaxLives,
        //        _match.MatchSettings.DefaultPlayerSpeed);
        //    //trainComponent.TrainState = trainState;
        //    trainComponent.SubscribeToCollision(HandleCollisionCallback);
        //    trainComponent.TrainName = player.Name;
        //    trainComponent.Speed = trainState.Speed;
        //    trainState.SubscribeToModelChanged(PlayerStateChangedCallback);
        //    //playerStates.Add(player.Name, trainState);
        //    _trains.Add(player.Name, trainComponent);
        //    //ClientScene.RegisterPrefab(trainSource);
        //    NetworkServer.AddPlayerForConnection(conn, trainObject, playerControllerId);
        //}

        //public void SetMatch(Match match)
        //{
        //    _match = match;
        //    _spawnPoints = new Dictionary<string, Transform>();
        //    _trains = new Dictionary<string, Train>();
        //    _collisionCenter = new CollisionController();

        //    var mapPath = string.Format("{0}{1}", PATH_MAPS, match.MapSettings.Name);
        //    var mapSource = Resources.Load(mapPath) as GameObject;
        //    var playerStates = new Dictionary<string, PlayerState>();

        //    _mapObject = GameObject.Instantiate(mapSource);

        //    _powerupHandler = new PowerupHandler(match, PATH_POWERUPS);
        //    //_matchState = new MatchState(playerStates);

        //    _collisionCenter.AddCollisionHandler(new PowerupCollisionHandler(_powerupHandler));
        //    _collisionCenter.AddCollisionHandler(new ObstacleCollisionHandler(RespawnTrain));

        //    //match.Start(_matchState);
        //}

        //public void RespawnTrain(Train train)
        //{
        //    train.Explode();

        //    train.Reset(_spawnPoints[train.TrainName].position,
        //        _spawnPoints[train.TrainName].rotation);
        //}

        //public void PlayAction(List<PlayerAction> actions)
        //{
        //    for (int i = 0; i < actions.Count; i++)
        //    {
        //        _trains[actions[i].PlayerName].Play(actions[i].Type);
        //    }
        //}

        //public void RunPowerUpSpawner(float deltaTime)
        //{
        //    _powerupHandler.Update(deltaTime);
        //}

        //private void HandleCollisionCallback(object sender, CollisionEventArgs args)
        //{
        //    args.MatchState = _matchState;
        //    _collisionCenter.HandleCollision(sender, args);
        //}

        //private void PlayerStateChangedCallback(object sender, ModelChangedEventArgs args)
        //{
        //    PlayerState newState = (PlayerState)sender;
        //    if (newState == null)
        //        return;
        //    Train trainSender = _trains[newState.TrainName];
        //    trainSender.Speed = newState.Speed;
        //}
    }
}