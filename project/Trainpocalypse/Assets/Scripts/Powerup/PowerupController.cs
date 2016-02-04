﻿using System;
using System.Collections.Generic;
using System.Linq;
using Funk.Data;
using Funk.Collision;

namespace Funk.Powerup
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

        private const int MAX_SPAWN_TRIES = 5;

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
            int rezX = _mapData.Width / 2;
            int rezY = _mapData.Height / 2;
            int tries = 0;
            do
            {
                x = UnityEngine.Random.Range(-rezX, rezX);
                y = UnityEngine.Random.Range(-rezY, rezY);
                tries++;
            } while (!_spawner.CanSpawn(x, y) && tries < MAX_SPAWN_TRIES);

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
                _totalSpawned++;
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

        public void PickUpEventHandler(object sender, CollisionEventArgs args)
        {
            PowerupBase pw = args.Other.GetComponent<PowerupBase>();
            Type powerupType = pw.GetType();
            if (_spawnedInstances.ContainsKey(powerupType))
            {
                PickUp(powerupType);
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
