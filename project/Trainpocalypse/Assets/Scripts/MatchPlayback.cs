using Funk.Collision;
using Funk.Data;
using Funk.Powerup;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

namespace Funk
{
    public class MatchPlayback : NetworkLobbyManager
    {
        [SerializeField]
        private Text _info;

        private const string PATH_BLOCKS = "Blocks/";
        private const string PATH_TRAINS = "Trains/";
        private const string PATH_MAPS = "Maps/";
        private const string PATH_POWERUPS = "Powerups";

        private Dictionary<string, Train> _trains;
        private Dictionary<string, Transform> _spawnPoints;
        private CollisionController _collisionCenter;
        private PowerupHandler _powerupHandler;

        private MatchState _matchState;
        private Match _match;
        private GameObject _mapObject;

        private void Awake()
        {
            StartMatchMaker();
            matchMaker.ListMatches(0, 1, "", OnMatchList);
        }       

        public override void OnMatchList(ListMatchResponse response)
        {
            base.OnMatchList(response);

            if (response.matches.Count > 0)
            {
                Debug.Log("joining match");
                matchMaker.JoinMatch(response.matches[0].networkId, "", OnMatchJoined);
            }
            else
            {
                Debug.Log("creating match");
                matchMaker.CreateMatch("wololo", (uint)maxPlayers, true, "", OnMatchCreate);
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn,
            short playerControllerId)
        {
            Debug.Log(".|.");
            PlayerSettings player = _match.PlayerSettings.First(p => p.Index == playerControllerId);
            _spawnPoints.Add(player.Name,
                   _mapObject.transform.Find("Spawn" + player.Index.ToString()));

            Debug.Log(playerControllerId);

            var trainPath = string.Format("{0}{1}", PATH_TRAINS, player.Train);
            var trainSource = Resources.Load(trainPath) as GameObject;
            var trainObject = GameObject.Instantiate(
                trainSource,
                _spawnPoints[player.Name].position,
                _spawnPoints[player.Name].rotation
                ) as GameObject;

            var trainComponent = trainObject.GetComponent<Train>();
            var trainState = new PlayerState(player.Name, _match.MatchSettings.MaxLives,
                _match.MatchSettings.DefaultPlayerSpeed);
            //trainComponent.TrainState = trainState;
            trainComponent.SubscribeToCollision(HandleCollisionCallback);
            trainComponent.TrainName = player.Name;
            trainComponent.Speed = trainState.Speed;
            trainState.SubscribeToModelChanged(PlayerStateChangedCallback);
            //playerStates.Add(player.Name, trainState);
            _trains.Add(player.Name, trainComponent);
            //ClientScene.RegisterPrefab(trainSource);
            NetworkServer.AddPlayerForConnection(conn, trainObject, playerControllerId);
        }

        public void SetMatch(Match match)
        {
            _match = match;
            _spawnPoints = new Dictionary<string, Transform>();
            _trains = new Dictionary<string, Train>();
            _collisionCenter = new CollisionController();

            var mapPath = string.Format("{0}{1}", PATH_MAPS, match.MapSettings.Name);
            var mapSource = Resources.Load(mapPath) as GameObject;
            var playerStates = new Dictionary<string, PlayerState>();

            _mapObject = GameObject.Instantiate(mapSource);

            _powerupHandler = new PowerupHandler(match, PATH_POWERUPS);
            //_matchState = new MatchState(playerStates);

            _collisionCenter.AddCollisionHandler(new PowerupCollisionHandler(_powerupHandler));
            _collisionCenter.AddCollisionHandler(new ObstacleCollisionHandler(RespawnTrain));

            //match.Start(_matchState);
        }

        public void RespawnTrain(Train train)
        {
            train.Explode();

            train.Reset(_spawnPoints[train.TrainName].position,
                _spawnPoints[train.TrainName].rotation);
        }

        public void PlayAction(List<PlayerAction> actions)
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

        private void HandleCollisionCallback(object sender, CollisionEventArgs args)
        {
            args.MatchState = _matchState;
            _collisionCenter.HandleCollision(sender, args);
        }

        private void PlayerStateChangedCallback(object sender, ModelChangedEventArgs args)
        {
            PlayerState newState = (PlayerState)sender;
            if (newState == null)
                return;
            Train trainSender = _trains[newState.TrainName];
            trainSender.Speed = newState.Speed;
        }
    }
}