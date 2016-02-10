using System;
using System.Collections.Generic;
using System.Linq;
using Funk.Data;
using UnityEngine;

namespace Funk.Powerup
{
    public class PowerupHandler
    {
        private MatchSettings _matchSettings;
        private MapSettings _mapSettings;

        private Dictionary<Type, GameObject> _allPowerups;
        private Dictionary<Type, int> _spawnedInstances;
        private Dictionary<Type, float> _spawnedCooldowns;
        private int _totalSpawned;

        private const int MAX_SPAWN_TRIES = 5;

        public PowerupHandler(Match match, string powerupPath)
        {
            _matchSettings = match.MatchSettings;
            _mapSettings = match.MapSettings;
            _spawnedInstances = new Dictionary<Type, int>();
            _spawnedCooldowns = new Dictionary<Type, float>();

            LoadPowerups(match.MatchSettings.PowerupsAvailable, powerupPath);
        }

        public List<Type> GetAvailablePowerups()
        {
            Type[] powerupTypes = _matchSettings.PowerupsAvailable.ToArray();
            Type[] powerupsOnCooldown = _spawnedCooldowns.Keys.ToArray();
            List<Type> availablePowerups = new List<Type>();

            for (int i = 0; i < powerupTypes.Length; i++)
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
            int rezX = _mapSettings.Width / 2;
            int rezY = _mapSettings.Height / 2;
            int tries = 0;
            do
            {
                x = UnityEngine.Random.Range(-rezX, rezX);
                y = UnityEngine.Random.Range(-rezY, rezY);
                tries++;
            } while (!CanSpawn(x, y) && tries < MAX_SPAWN_TRIES);

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

                Spawn(x, y, availablePowerups[k]);

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
                _totalSpawned++;
            }
        }

        public void Update(float deltaTime)
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

        private void LoadPowerups(IEnumerable<Type> powerupTypes, string path)
        {
            _allPowerups = new Dictionary<Type, GameObject>();
            PowerupBase[] resourcesPowerups = Resources.LoadAll<PowerupBase>(path);
            foreach (var p in resourcesPowerups)
            {
                bool shouldAdd = false;
                foreach (var pt in powerupTypes)
                {
                    if (p.GetType() == pt)
                    {
                        shouldAdd = true;
                        break;
                    }
                }
                if (shouldAdd)
                {
                    _allPowerups.Add(p.GetType(), p.gameObject);
                }
            }
            resourcesPowerups = null;
            Resources.UnloadUnusedAssets();
        }

        private bool CanSpawn(int x, int y)
        {
            RaycastHit hit;
            bool bHit = Physics.SphereCast(new Vector3(x, 1, y), 0.5f, -Vector3.up, out hit, 1f);
            return !bHit;
        }

        private void Spawn(int x, int y, Type powerupType)
        {
            GameObject.Instantiate(_allPowerups[powerupType],
                new Vector3(x, 0, y), Quaternion.identity);
        }
    }
}
