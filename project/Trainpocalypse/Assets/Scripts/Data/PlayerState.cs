using Funk.Player;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Funk.Data
{
    public class PlayerState : IObservableModel
    {
        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                if (ModelChanged != null)
                {
                    ModelChanged.Invoke(this,
                        new ModelChangedEventArgs(this.GetType().GetProperty("Speed")));
                }
            }
        }

        public float DominoDelay
        {
            get
            {
                return _dominoDelay;
            }
            set
            {
                _dominoDelay = value;
                if (ModelChanged != null)
                {
                    ModelChanged.Invoke(this,
                        new ModelChangedEventArgs(this.GetType().GetProperty("DominoDelay")));
                }
            }
        }
        public bool IsInvulnerable
        {
            get
            {
                return _isInvulnerable;
            }
            set
            {
                _isInvulnerable = value;
                if (ModelChanged != null)
                {
                    ModelChanged.Invoke(this,
                        new ModelChangedEventArgs(this.GetType().GetProperty("IsInvulnerable")));
                }
            }
        }

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

        public string TrainName { get; private set; }

        public IEnumerable<IStateModifier> ActiveModifiers
        {
            get { return _activePowerups; }
        }

        private int _currentLives;
        private float _speed;
        private float _dominoDelay;
        private bool _isInvulnerable;
        private float _defaultSpeed;
        private float _defaultDominoDelay;
        private int _startingLives;
        private List<IStateModifier> _activePowerups;

        public event EventHandler<ModelChangedEventArgs> ModelChanged;

        public PlayerState(string name, int startingLives, float defaultSpeed)
        {
            _startingLives = startingLives;
            _defaultSpeed = defaultSpeed;
            _defaultDominoDelay = 0f;
            TrainName = name;
            Reset();
        }

        public void Reset()
        {
            _activePowerups = new List<IStateModifier>();
            _speed = _defaultSpeed;
            _dominoDelay = _defaultDominoDelay;
            _currentLives = _startingLives;
            _isInvulnerable = false;
        }

        public void AddModifier(IStateModifier powerup)
        {
            _activePowerups.Add(powerup);
        }

        public void RemoveModifier(IStateModifier powerup)
        {
            _activePowerups.Remove(powerup);
        }

        public void ResetPowerups()
        {
            for (int i = 0; i < _activePowerups.Count; i++)
            {
                _activePowerups[i].Unapply();
            }
            _activePowerups = new List<IStateModifier>();
        }

        public void SubscribeToModelChanged(EventHandler<ModelChangedEventArgs> action)
        {
            ModelChanged += action;
        }
    }
}
