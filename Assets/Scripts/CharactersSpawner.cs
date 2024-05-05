using LearnGame.Camera;
using LearnGame.Enemy;
using UnityEditor;
using UnityEngine;

namespace LearnGame
{
    [RequireComponent(typeof(CharactersSpawnerController))]
    public class CharactersSpawner : CharactersSpawnerController
    {
        [SerializeField]
        private EnemyCharacter _enemyPrefab;

        [SerializeField]
        private PlayerCharacter _playerPrefab;

        [SerializeField]
        private float _range = 2f;

        [SerializeField]
        private int _maxCountCharactersOnMap = 5; //1 слот под игрока зарезервирован

        [SerializeField]
        private float _minspawnIntervalSeconds = 10f;

        [SerializeField]
        private float _maxspawnIntervalSeconds = 15f;

        private float _currentSpawnTimerSeconds;
        private float _spawnIntervalSeconds = 0;
        private readonly Collider[] _colliders = new Collider[100];

        protected void Awake()
        {
            var randomPointInsideRange = Random.insideUnitCircle * _range;
            var randomPosition = new Vector3(randomPointInsideRange.x, 1f, randomPointInsideRange.y) + transform.position;
            if (!isPlayerAlive())
            {
                var player = Instantiate(_playerPrefab, randomPosition, Quaternion.identity, transform);
                var camera = FindObjectOfType<CameraController>();
                camera.setPlayer(player);
            }
        }

        protected void Update()
        {
            _currentSpawnTimerSeconds += Time.deltaTime;
            if (_currentSpawnTimerSeconds > _spawnIntervalSeconds && CharsOnSpawn() == 0)
            {
                _currentSpawnTimerSeconds = 0f;
                _spawnIntervalSeconds = Random.Range(_minspawnIntervalSeconds, _maxspawnIntervalSeconds);
                var randomPointInsideRange = Random.insideUnitCircle * _range;
                var randomPosition = new Vector3(randomPointInsideRange.x, 1f, randomPointInsideRange.y) + transform.position;
                var decision = Random.Range(0, 100);
                if (decision <= 50 && !isPlayerAlive())
                {
                    var player = Instantiate(_playerPrefab, randomPosition, Quaternion.identity, transform);
                    var camera = FindObjectOfType<CameraController>();
                    camera.setPlayer(player);
                } else if (decision > 50 && GetCharactersNumber() < _maxCountCharactersOnMap - 1)
                {
                    Instantiate(_enemyPrefab, randomPosition, Quaternion.identity, transform);
                }
                //Debug.Log(GetCharactersNumber());
            }
        }

        protected void OnDrawGizmos()
        {
            var cashedColor = Handles.color;
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, _range);
            Handles.color = cashedColor;
        }

        protected int CharsOnSpawn()
        {
            var size = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _range, _colliders, LayerUtils.CharactersMask);
            return size;
        }

    }
}