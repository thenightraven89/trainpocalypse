using Funk.Powerup;
using System.Collections.Generic;
using System.Linq;

namespace Funk.Data
{
    public class PlayerState
    {
        public float Speed { get; set; }
        public float DominoDelay { get; set; }
        public bool IsInvulnerable { get; set; }
        public Train TrainController { get; set; }
        public int Lives
        {
            get
            {
                return _currentLives;
            }
            set
            {
                if (value > _startingLives)
                {
                    _currentLives = _startingLives;
                }
                else
                {
                    _currentLives = value;
                }
            }
        }
        public bool IsDead { get { return Lives <= 0f; } }
        public IEnumerable<PowerupBase> ActivePowerups
        {
            get { return _activePowerups; }
        }

        private int _currentLives;
        private float _defaultSpeed;
        private float _defaultDominoDelay;
        private int _startingLives;
        private List<PowerupBase> _activePowerups;

        public PlayerState(Train train, int startingLives, float defaultSpeed)
        {
            TrainController = train;
            _startingLives = startingLives;
            _defaultSpeed = defaultSpeed;
            _defaultDominoDelay = 0f;
            Reset();
        }

        public void Reset()
        {
            _activePowerups = new List<PowerupBase>();
            Speed = _defaultSpeed;
            DominoDelay = _defaultDominoDelay;
            _currentLives = _startingLives;
            IsInvulnerable = false;
        }

        public void AddActivePowerup(PowerupBase powerup)
        {
            _activePowerups.Add(powerup);
        }

        public void RemoveActivePowerup(PowerupBase powerup)
        {
            _activePowerups.Remove(powerup);
        }

        public void ResetPowerups()
        {
            for (int i = 0; i < _activePowerups.Count; i++)
            {
                _activePowerups[i].Unapply();
            }
            _activePowerups = new List<PowerupBase>();
        }
    }
}
