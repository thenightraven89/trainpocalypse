using Funk.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
    public class Train : MonoBehaviour, IActionProvider
    {
        private const string PREFIX_AXIS_HORIZONTAL = "Horizontal";
        private const string PREFIX_AXIS_VERTICAL = "Vertical";

        private string _inputSuffix;

        private ICollection<IPlaybackAction> _actions;

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

        private float _axisH;
        private float _axisV;

        private void Start()
        {
            _transform = transform;
            _direction = _transform.forward;
            _dominoTrail = new List<GameObject>();
            _actions = new List<IPlaybackAction>();
            _moveCoroutine = StartCoroutine(Move());
        }

        public void AssignInputMap(InputMap inputMap)
        {
            _inputSuffix = ((byte)inputMap).ToString();
        }

        private void Update()
        {
            _axisH = Input.GetAxis(PREFIX_AXIS_HORIZONTAL + _inputSuffix);
            _axisV = Input.GetAxis(PREFIX_AXIS_VERTICAL + _inputSuffix);

            if (_axisH < 0 && _direction != Vector3.left)
                _direction = Vector3.left;

            if (_axisH > 0 && _transform.forward != Vector3.right)
                _direction = Vector3.right;

            if (_axisV > 0 && _transform.forward != Vector3.back)
                _direction = Vector3.back;

            if (_axisV < 0 && _transform.forward != Vector3.forward)
                _direction = Vector3.forward;
        }

        private IEnumerator Move()
        {
            while (true)
            {
                if (_cachedSteeringData != null)
                {
                    _transform.position = _cachedSteeringData.Position;
                    _transform.rotation = Quaternion.LookRotation(
                        _cachedSteeringData.Direction,
                        Vector3.up);

                    _cachedSteeringData = null;
                }
                
                _from = _transform.position;
                _to = _from + _transform.forward * _tileSize;

                if (_direction != _transform.forward)
                {
                    _actions.Add(new TrainSteerAction(
                        TrainName,
                        _from,
                        _direction));
                }

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

        private SteeringData _cachedSteeringData = null;

        public class SteeringData
        {
            public Vector3 Position;
            public Vector3 Direction;
        }

        public void Steer(Vector3 position, Vector3 direction)
        {
            _cachedSteeringData = new SteeringData()
            {
                Position = position,
                Direction = direction
            };

            //_transform.position = position;
            //_transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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

        }

        public ICollection<IPlaybackAction> GetActions()
        {
            return _actions;
        }

        public void ClearActions()
        {
            _actions.Clear();
        }
    }
}