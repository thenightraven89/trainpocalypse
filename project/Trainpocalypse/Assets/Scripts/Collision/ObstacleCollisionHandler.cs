using Funk.Data;
using Funk.Powerup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Funk.Collision
{
    class ObstacleCollisionHandler : ICollisionHandler
    {
        public string Tag
        {
            get
            {
                return "Obstacle";
            }
        }

        private Action<Train> _respawnMethod;

        public ObstacleCollisionHandler(Action<Train> respawnMethod)
        {
            _respawnMethod = respawnMethod;
        }

        public void HandleCollision(ICollisionTirgger t, Collider other, MatchState state)
        {
            Train trainSender = (Train)t;
            if (t == null)
                return;

            trainSender.TrainState.Lives--;

            if (trainSender.TrainState.IsDead)
            {
                GameObject.Destroy(trainSender.gameObject);
            }
            else
            {
                _respawnMethod.Invoke(trainSender);
            }
        }
    }
}
