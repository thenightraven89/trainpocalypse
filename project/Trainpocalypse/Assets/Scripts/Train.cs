using Funk.Data;
using Funk.Collision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

namespace Funk
{
    public class Train : NetworkBehaviour, ICollisionTirgger
    {
        public string TrainName { get; set; }
        public float Speed { get; set; }

        [SerializeField]
        private GameObject _domino;

        private List<GameObject> _dominoTrail;
        private Vector3 _from;
        private Vector3 _to;
        private Vector3 _direction;
        private float _tileSize = 1f;
        private Transform _transform;
        private Coroutine _moveCoroutine;

        public event EventHandler<CollisionEventArgs> OnTrigger;

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
                    _from = Vector3.MoveTowards(_from, _to, Speed * Time.deltaTime);
                    _transform.position = _from;
                    yield return null;
                }

                _transform.position = _from;

                var newBlock = GameObject.Instantiate(
                    _domino,
                    _domino.transform.position,
                    _domino.transform.rotation) as GameObject;

                newBlock.SetActive(true);
                _dominoTrail.Add(newBlock);
            }
        }

        public void Play(ActionType action)
        {
            if (!isLocalPlayer)
            {
                return;
            }
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

        public void Explode()
        {
            //Spawn a mock train model with some kind of death animation that will
            //quickly fade.
        }

        public void Reset(Vector3 position, Quaternion rotation)
        {
            StopCoroutine(_moveCoroutine);
            _transform.position = position;
            _transform.rotation = rotation;
            Start();
        }

        public void SubscribeToCollision(EventHandler<CollisionEventArgs> action)
        {
            OnTrigger += action;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (OnTrigger != null)
            {
                OnTrigger.Invoke(this, new CollisionEventArgs(this, other));
            }
        }
    }
}