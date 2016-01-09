using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Funk
{
    public class PowerupSpawner : MonoBehaviour
    {
        private Dictionary<Type, GameObject> _allPowerups;

        public void LoadPowerups(IEnumerable<Type> powerupTypes)
        {
            _allPowerups = new Dictionary<Type, GameObject>();
            PowerupBase[] resourcesPowerups = Resources.LoadAll<PowerupBase>("Powerups");
            Debug.Log(resourcesPowerups.Length);
            foreach (var p in resourcesPowerups)
            {
                bool shouldAdd = false;
                foreach (var pt in powerupTypes)
                {
                    if (p.GetType() == pt)
                    {
                        shouldAdd = true;
                        
                    }
                }
                if (shouldAdd)
                {
                    _allPowerups.Add(p.GetType(), p.gameObject);
                }
            }
        }

        public bool CanSpawn(int x, int y)
        {
            RaycastHit hit;
            bool bHit = Physics.SphereCast(new Vector3(x,1,y), 0.5f, -Vector3.up, out hit, 1f);
            return !bHit;
        }

        public void Spawn(int x, int y, Type powerupType)
        {
            Debug.Log("Spawned " + powerupType.ToString());
            Instantiate(_allPowerups[powerupType], new Vector3(x, 0, y), Quaternion.identity);
        }
    }
}
