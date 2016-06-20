using UnityEngine;

namespace Funk.Data
{
    public interface IPlaybackAction
    {

    }

    public class TrainSteerAction : IPlaybackAction
    {
        public string TrainId { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }

        public TrainSteerAction(
            string trainId,
            Vector3 position,
            Vector3 direction)
        {
            TrainId = trainId;
            Position = position;
            Direction = direction;
        }

        public override string ToString()
        {
            return "Steering " + Position + " towards " + Direction;
        }
    }

    public class TrainExplodeAction : IPlaybackAction
    {
        public string TrainId { get; private set; }

        public TrainExplodeAction(string trainId)
        {
            TrainId = trainId;
        }
    }

    public class TrainSpawnAction : IPlaybackAction
    {
        public PlayerSettings Settings { get; private set; }

        public TrainSpawnAction(PlayerSettings settings)
        {
            Settings = settings;
        }
    }

    public class PowerupSpawnAction : IPlaybackAction
    {
        public GameObject PowerUp { get; private set; }
        public Vector3 Position { get; private set; }

        public PowerupSpawnAction(GameObject powerUp, Vector3 position)
        {
            PowerUp = powerUp;
            Position = position;
        }
    }

    public class MapLoadAction : IPlaybackAction
    {
        public MapSettings Settings { get; private set; }

        public MapLoadAction(MapSettings settings)
        {
            Settings = settings;
        }
    }
}