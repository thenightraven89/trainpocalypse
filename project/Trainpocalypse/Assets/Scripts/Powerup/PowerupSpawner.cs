using UnityEngine;
using System.Collections.Generic;
using System;

namespace Funk.Powerup
{
    public class PowerupSpawner
    {
        private Dictionary<Type, GameObject> _allPowerups;

        public PowerupSpawner(IEnumerable<Type> powerupTypes, string powerupPath)
        {
            LoadPowerups(powerupTypes, powerupPath);
        }

        public void LoadPowerups(IEnumerable<Type> powerupTypes, string path)
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

        public bool CanSpawn(int x, int y)
        {
            RaycastHit hit;
            bool bHit = Physics.SphereCast(new Vector3(x,1,y), 0.5f, -Vector3.up, out hit, 1f);
            return !bHit;
        }

        public void Spawn(int x, int y, Type powerupType)
        {
            GameObject.Instantiate(_allPowerups[powerupType], 
                new Vector3(x, 0, y), Quaternion.identity);
        }
    }
}
