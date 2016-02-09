using Funk.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Funk.Collision
{
    public class CollisionController
    {
        private Dictionary<string, ICollisionHandler> _collisionHandlers;

        public CollisionController()
        {
            _collisionHandlers = new Dictionary<string, ICollisionHandler>();
        }

        public CollisionController(Match match, IEnumerable<ICollisionHandler> collisionHandlers)
        {
            _collisionHandlers = new Dictionary<string, ICollisionHandler>();
            foreach (var handler in collisionHandlers)
            {
                _collisionHandlers.Add(handler.Tag, handler);
            }
        }

        public void AddCollisionHandler(ICollisionHandler handler)
        {
            _collisionHandlers.Add(handler.Tag, handler);
        }

        public void HandleCollision(object sender, CollisionEventArgs args)
        {
            string tag = args.Other.tag;
            if (_collisionHandlers.ContainsKey(tag))
            {
                _collisionHandlers[tag].HandleCollision((ICollisionTirgger)sender, args);
            }
        }
    }

    public class CollisionEventArgs : EventArgs
    {
        public Collider Other { get; private set; }

        public Train SenderTrain { get; private set; }

        public MatchState MatchState { get; set; }

        public CollisionEventArgs(Train sender, Collider other)
        {
            Other = other;
            SenderTrain = sender;
        }
    }

    public interface ICollisionHandler
    {
        string Tag { get; }

        void HandleCollision(ICollisionTirgger t, CollisionEventArgs collisionArgs);
    }

    public interface ICollisionTirgger
    {
        event EventHandler<CollisionEventArgs> OnTrigger;

        void SubscribeToCollision(EventHandler<CollisionEventArgs> action);
    }
}
