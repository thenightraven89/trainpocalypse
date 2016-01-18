﻿using UnityEngine;
using System.Collections;
using Funk.Data;

namespace Funk.Powerup
{
    public class PowerupBase : MonoBehaviour
    {
        [SerializeField]
        protected float _duration;
        [SerializeField]
        protected GameObject _visuals;
        [SerializeField]
        protected Collider _collider;

        protected MatchState _matchContext;
        protected Train _powerupTarget;
        protected PlayerState _affectedPlayerState;
        private Coroutine _waitCoroutine;

        public void Apply(Train target, MatchState context)
        {
            _matchContext = context;
            _powerupTarget = target;
            for (int i = 0; i <= context.PlayersStates.Length; i++)
            {
                if (context.PlayersStates[i].TrainController == target)
                {
                    _affectedPlayerState = context.PlayersStates[i];
                    break;
                }
            }
            _affectedPlayerState.AddActivePowerup(this);
            Hide();
            ApplyEffect();
            _waitCoroutine = StartCoroutine(WaitToUnapply());
        }

        private void Hide()
        {
            _collider.enabled = false;
            _visuals.SetActive(false);
        }

        protected virtual void ApplyEffect()
        {
            Debug.Log("Much powerup such pickup wow");
        }

        private IEnumerator WaitToUnapply()
        {
            yield return new WaitForSeconds(_duration);
            _waitCoroutine = null;
            Unapply();
        }

        public void Unapply()
        {
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
            }
            else
            {
                _affectedPlayerState.RemoveActivePowerup(this);
            }
            UnapplyEffect();
            Destroy(gameObject);
        }

        protected virtual void UnapplyEffect()
        {
        }
    }
}
