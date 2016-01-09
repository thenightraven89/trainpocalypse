using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Funk
{
    public class PowerupSpawner : MonoBehaviour
    {
        private Dictionary<Type, PowerupBase> _allPowerups;

        public void LoadPowerups(IEnumerable<Type> powerupTypes)
        {
            _allPowerups = new Dictionary<Type, PowerupBase>();
        }

        public bool CanSpawn(int x, int y)
        {
            return true;
        }

        public void Spawn(int x, int y, Type powerupType)
        {
            Debug.Log("Spawned " + powerupType.ToString());
        }
    }
}
