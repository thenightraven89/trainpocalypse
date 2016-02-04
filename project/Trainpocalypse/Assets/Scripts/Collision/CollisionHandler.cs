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
        private Match _match;
        private Dictionary<string, ICollisionHandler> _collisionHandlers;

        public CollisionController(Match match)
        {
            _match = match;
            _collisionHandlers = new Dictionary<string, ICollisionHandler>();
        }

        public CollisionController(Match match, IEnumerable<ICollisionHandler> collisionHandlers)
        {
            _match = match;
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
                _collisionHandlers[tag].HandleCollision((ICollisionTirgger)sender, args.Other,
                    _match.MatchState);
            }
        }
    }

    public class CollisionEventArgs : EventArgs
    {
        public Collider Other { get; private set; }

        public CollisionEventArgs(Collider other)
        {
            Other = other;
        }
    }

    public interface ICollisionHandler
    {
        string Tag { get; }

        void HandleCollision(ICollisionTirgger t, Collider other, MatchState context);
    }

    public interface ICollisionTirgger
    {
        event EventHandler<CollisionEventArgs> OnTrigger;

        void SubscribeToCollision(EventHandler<CollisionEventArgs> action);
    }
}
