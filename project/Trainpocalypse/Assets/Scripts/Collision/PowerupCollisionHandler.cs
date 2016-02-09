﻿using Funk.Data;
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

        private PowerupController _powerupController;

        public PowerupCollisionHandler(PowerupController controller)
        {
            _powerupController = controller;
        }

        public void HandleCollision(ICollisionTirgger t, CollisionEventArgs collisionArgs)
        {
            Train trainSender = (Train)t;
            if (t==null)
                return;

            PowerupBase pw = collisionArgs.Other.GetComponent<PowerupBase>();

            pw.Apply(new ApplyEffectContext(trainSender, collisionArgs.MatchState));

            _powerupController.PickUp(pw.GetType());
        }
    }
}
