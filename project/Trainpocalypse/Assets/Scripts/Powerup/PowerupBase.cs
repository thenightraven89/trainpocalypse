using UnityEngine;
using System.Collections;
using Funk.Data;

namespace Funk.Powerup
{
    public class PowerupBase : MonoBehaviour
    {
        [SerializeField]
        private float _duration;
        private MatchState _matchContext;
        private Train _powerupTarget;
        private PlayerState _affectedPlayerState;
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
            ApplyEffect();
            _waitCoroutine = StartCoroutine(WaitToUnapply());
        }

        protected virtual void ApplyEffect()
        {

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
        }

        protected virtual void UnapplyEffect()
        {
        }
    }
}
