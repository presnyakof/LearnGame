using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LearnGame.PickUp
{
    public class PickUpSpawner : MonoBehaviour
    {
        [SerializeField]
        private PickUpItem _pickUpPrefab;

        [SerializeField]
        private float _range = 2f;

        [SerializeField]
        private int _maxCount = 2;

        [SerializeField]
        private float _minspawnIntervalSeconds = 10f;

        [SerializeField]
        private float _maxspawnIntervalSeconds = 15f;


        private float _currentSpawnTimerSeconds;
        private float _spawnIntervalSeconds = 0;
        private int _currentCount;
        private readonly Collider[] _colliders = new Collider[10];


        protected void Update()
        {
            if (_currentCount < _maxCount)
            {
                _currentSpawnTimerSeconds += Time.deltaTime;
                if(_currentSpawnTimerSeconds > _spawnIntervalSeconds)
                {
                    _currentSpawnTimerSeconds = 0f;
                    _currentCount++;
                    _spawnIntervalSeconds = Random.Range(_minspawnIntervalSeconds, _maxspawnIntervalSeconds);
                    var randomPointInsideRange = Random.insideUnitCircle * _range;
                    var randomPosition = new Vector3(randomPointInsideRange.x, 0f, randomPointInsideRange.y) + transform.position;
                    var pickUp = Instantiate(_pickUpPrefab, randomPosition, Quaternion.identity, transform);
                    pickUp.OnPickUp += OnItemPickUp;
                }
            }
        }
        private void OnItemPickUp(PickUpItem pickUpitem)
        {
            _currentCount--;
            pickUpitem.OnPickUp -= OnItemPickUp;
        }

        protected void OnDrawGizmos()
        {
            var cashedColor = Handles.color;
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, _range);
            Handles.color = cashedColor;
        }

    }
}