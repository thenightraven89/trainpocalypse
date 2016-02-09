using UnityEngine;
using System.Collections;
using Funk.Data;
using Funk.Player;

namespace Funk.Powerup
{
    public class PowerupBase : MonoBehaviour, IStateModifier
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

        public void Apply(ApplyEffectContext context)
        {
            _matchContext = context.Context;
            _powerupTarget = context.Target;

            _affectedPlayerState = _matchContext.GetPlayerState(_powerupTarget.TrainName);
                   
            _affectedPlayerState.AddModifier(this);
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
                _affectedPlayerState.RemoveModifier(this);
            }
            UnapplyEffect();
            Destroy(gameObject);
        }

        protected virtual void UnapplyEffect()
        {

        }
    }
}
