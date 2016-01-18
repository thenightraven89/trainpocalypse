using Funk.Data;
using Funk.Powerup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Funk.Collision
{
    class PowerupCollisionHandler : ICollisionHandler
    {
        public string Tag
        {
            get
            {
                return "Powerup";
            }
        }

        public event EventHandler<CollisionEventArgs> OnCollision;

        public void HandleCollision(ICollisionTirgger t, Collider other, MatchState state)
        {
            Train trainSender = (Train)t;
            if (t==null)
                return;
            if (OnCollision != null)
            {
                OnCollision.Invoke(t, new CollisionEventArgs(other));
            }
            PowerupBase pw = other.GetComponent<PowerupBase>();
            pw.Apply(trainSender, state);
        }

        public void SubscribeToOnCollision(EventHandler<CollisionEventArgs> action)
        {
            OnCollision += action;
        }
    }
}
