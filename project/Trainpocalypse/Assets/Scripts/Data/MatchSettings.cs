using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funk.Data
{
    public class MatchSettings
    {
        public int MaxLives { get; private set; }
        public int MaxSpawnedPowerups { get; private set; }
        public int DefaultPlayerSpeed { get; private set; }

        public IEnumerable<Type> PowerupsAvailable{ get { return _powerupSettings.Keys; } }
        private Dictionary<Type, PowerupSettings> _powerupSettings;

        public MatchSettings(int maxLives, int maxSpawnedPowerups, 
            IEnumerable<PowerupSettings> powerupSettings)
        {
            MaxLives = maxLives;
            MaxSpawnedPowerups = maxSpawnedPowerups;
            _powerupSettings = new Dictionary<Type, PowerupSettings>();
            foreach (var pSetting in powerupSettings)
            {
                _powerupSettings.Add(pSetting.Powerup, pSetting);
            }
        }

        public PowerupSettings GetPowerupSettings(Type powerupType)
        {
            return _powerupSettings[powerupType];
        }
    }
}
