using Funk.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
    public class Train : MonoBehaviour
    {
        [SerializeField]
        private GameObject _domino;

        private List<GameObject> _dominoTrail;
        private float _speed = 8f;
        private Vector3 _from;
        private Vector3 _to;
        private Vector3 _direction;
        private float _tileSize = 1f;
        private Transform _transform;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            _transform = transform;
            _direction = _transform.forward;
            _dominoTrail = new List<GameObject>();
            _moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            while (true)
            {
                _from = _transform.position;
                _to = _from + _direction * _tileSize;
                _transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);

                while (_from != _to)
                {
                    _from = Vector3.MoveTowards(_from, _to, _speed * Time.deltaTime);
                    _transform.position = _from;
                    yield return null;
                }

                _transform.position = _from;

                var newBlock = GameObject.Instantiate(
                    _domino,
                    _transform.position,
                    _transform.rotation) as GameObject;

                newBlock.SetActive(true);
                _dominoTrail.Add(newBlock);
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