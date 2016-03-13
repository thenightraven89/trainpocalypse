using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

namespace Funk
{
    public class Train : NetworkBehaviour
    {
        public string TrainName { get; set; }
        public float Speed { get; set; }

        [SerializeField]
        private GameObject _domino;

        [SerializeField]
        private Transform _dominoSpawnPoint;

        private List<GameObject> _dominoTrail;
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
            Speed = 10f;

            ClientScene.RegisterPrefab(_domino);

            if (isServer)
                _moveCoroutine = StartCoroutine(Move());
        }
        
        private IEnumerator Move()
        {
            Debug.Log("started moving");

            while (true)
            {
                _from = _transform.position;
                _to = _from + _direction * _tileSize;
                _transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);

                //Debug.Log(_direction + " is the direction");

                while (_from != _to)
                {
                    //Debug.Log(_transform.position);
                    _from = Vector3.MoveTowards(_from, _to, Speed * Time.deltaTime);
                    _transform.position = _from;
                    yield return null;
                }

                _transform.position = _from;

                var newBlock = GameObject.Instantiate(
                    _domino,
                    _dominoSpawnPoint.position,
                    _dominoSpawnPoint.rotation) as GameObject;
                newBlock.GetComponent<DominoSync>().serverRotation = _dominoSpawnPoint.rotation;
                NetworkServer.Spawn(newBlock);
                newBlock.SetActive(true);
                _dominoTrail.Add(newBlock);
            }
        }
        
        void Update()
        {
            if (!isLocalPlayer) return;

            var axisH = Input.GetAxis("Horizontal");
            var axisV = Input.GetAxis("Vertical");

            //transform.Translate(new Vector3(axisH, 0, axisV));

            if (axisH < 0)
            {
                CmdPlay(ActionType.MoveLeft);
            }

            if (axisH > 0)
            {
                CmdPlay(ActionType.MoveRight);
            }

            if (axisV < 0)
            {
                CmdPlay(ActionType.MoveDown);
            }

            if (axisV > 0)
            {
                CmdPlay(ActionType.MoveUp);
            }
        }

        [Command]
        public void CmdPlay(ActionType action)
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

        private void OnTriggerEnter(Collider other)
        {
            if (!isServer) return;

            if (other.CompareTag("Obstacle"))
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    public enum ActionType
    {
        None,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }
}