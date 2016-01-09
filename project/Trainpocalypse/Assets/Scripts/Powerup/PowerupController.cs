using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funk.Data;

namespace Funk
{
    class PowerupController
    {
        private Match _match;
        private MatchSettings _matchSettings;
        private MapData _mapData;
        private PowerupSpawner _spawner;
        private Dictionary<Type, int> _spawnedInstances;
        private Dictionary<Type, float> _spawnedCooldowns;
        private int _totalSpawned; 


        public PowerupController(Match match, PowerupSpawner spawner)
        {
            _match = match;
            _matchSettings = _match.MatchSettings;
            _mapData = _match.MapData;
            _spawner = spawner;
            _spawnedInstances = new Dictionary<Type, int>();
            _spawnedCooldowns = new Dictionary<Type, float>();
        }

        public List<Type> GetAvailablePowerups()
        {
            List<Type> powerupTypes = _matchSettings.PowerupsAvailable.ToList();
            List<Type> powerupsOnCooldown = _spawnedCooldowns.Keys.ToList();
            List<Type> availablePowerups = new List<Type>();

            for (int i = 0; i < powerupTypes.Count; i++)
            {
                int instances = 0;
                if (_spawnedInstances.ContainsKey(powerupTypes[i]))
                {
                    instances = _spawnedInstances[powerupTypes[i]];
                }
                if (!powerupsOnCooldown.Contains(powerupTypes[i]) && instances <
                    _matchSettings.GetPowerupSettings(powerupTypes[i]).MaxInstances)
                {
                    availablePowerups.Add(powerupTypes[i]);
                }
            }
            return availablePowerups;
        } 

        public void SpawnRandom()
        {
            int x, y;
            int rez = _mapData.Rezolution / 2;
            do
            {
                x = UnityEngine.Random.Range(-rez, rez);
                y = UnityEngine.Random.Range(-rez, rez);
            } while (!_spawner.CanSpawn(x, y));

            List<Type> availablePowerups = GetAvailablePowerups();

            if (availablePowerups.Count != 0)
            {
                float sum = 0;
                for (int i = 0; i < availablePowerups.Count; i++)
                {
                    sum += _matchSettings.GetPowerupSettings(availablePowerups[i]).Chance;
                }

                float r = UnityEngine.Random.Range(0, sum);
                float pSum = _matchSettings.GetPowerupSettings(availablePowerups[0]).Chance;
                int k = 0;
                while (pSum < r)
                {
                    k++;
                    pSum += _matchSettings.GetPowerupSettings(availablePowerups[k]).Chance;
                }

                _spawner.Spawn(x, y, availablePowerups[k]);

                if (_spawnedInstances.ContainsKey(availablePowerups[k]))
                {
                    _spawnedInstances[availablePowerups[k]]++;
                }
                else
                {
                    _spawnedInstances.Add(availablePowerups[k], 1);
                }

                _spawnedCooldowns.Add(availablePowerups[k],
                    _matchSettings.GetPowerupSettings(availablePowerups[0]).Cooldown);
            }
        }

        public void Run(float deltaTime)
        {
            var keys = new List<Type>(_spawnedCooldowns.Keys);
            foreach (var key in keys)
            {
                _spawnedCooldowns[key] -= deltaTime;
                if (_spawnedCooldowns[key] < 0f)
                {
                    _spawnedCooldowns.Remove(key);
                }
            }
            if (_totalSpawned < _matchSettings.MaxSpawnedPowerups)
            {
                SpawnRandom();
            }
        }

        public void PickUp(Type powerupType)
        {
            _totalSpawned--;
            _spawnedInstances[powerupType]--;
            if (_spawnedInstances[powerupType] <= 0)
            {
                _spawnedInstances.Remove(powerupType);
            }
        }
    }
}
