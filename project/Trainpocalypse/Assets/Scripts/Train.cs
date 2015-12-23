using Funk.Data;
using System.Collections;
using UnityEngine;

namespace Funk
{
    public class Train : MonoBehaviour
    {
        private float _speed = 8f;
        private Vector3 _from;
        private Vector3 _to;
        private Vector3 _direction;
        private Transform _transform;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            _transform = transform;
            _direction = _transform.forward;
            _moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            while (true)
            {
                _from = _transform.position;
                _to = _from + _direction;
                _transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);

                while (_from != _to)
                {
                    _from = Vector3.MoveTowards(_from, _to, _speed * Time.deltaTime);
                    _transform.position = _from;
                    yield return null;
                }

                _transform.position = _from;
            }
        }

        public void Play(ActionType action)
        {
            switch (action)
            {
                case ActionType.MoveUp:
                    _direction = Vector3.forward;
                    break;

                case ActionType.MoveDown:
                    _direction = Vector3.back;
                    break;

                case ActionType.MoveLeft:
                    _direction = Vector3.left;
                    break;

                case ActionType.MoveRight:
                    _direction = Vector3.right;
                    break;
            }
        }
    }
}