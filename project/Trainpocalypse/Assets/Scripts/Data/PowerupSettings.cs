using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funk.Data
{
    public class PowerupSettings
    {
        public Type Powerup { get; private set; }
        public int MaxInstances { get; private set; }
        public float Cooldown { get; private set; }
        public float Chance { get; private set; }

        public PowerupSettings(Type powerup, int maxInstances, float cooldown, float chance)
        {
            Powerup = powerup;
            MaxInstances = maxInstances;
            Cooldown = cooldown;
            Chance = chance; 
        }
    }
}
