using UnityEngine;
using LearnGame.Movement;
using LearnGame.Shooting;
using LearnGame.PickUp;
using static UnityEditor.PlayerSettings;
using UnityEditor;

namespace LearnGame 
{
    [RequireComponent(typeof(CharacterMovementController), typeof(ShootingController))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        [SerializeField]
        private Weapon _baseWeaponPrefab;
        [SerializeField]
        private Transform _hand;

        [SerializeField]
        private float _health = 2f;

        public float _maxHP { get; private set; }

        private bool _hasDefaultWeapon = true;


        private IMovementDirectionSource _movementDirectionSource;
        private CharacterMovementController _characterMovementController;
        private ShootingController _shootingController;
        protected void Awake()
        {
            _characterMovementController = gameObject.GetComponent<CharacterMovementController>();
            _movementDirectionSource = gameObject.GetComponent<IMovementDirectionSource>();
            _shootingController = gameObject.GetComponent<ShootingController>();
            _maxHP = _health;
        }

        protected void Start()
        {
            SetWeapon(_baseWeaponPrefab);
        }

        protected void Update()
        {
            var direction = _movementDirectionSource.MovementDirection;
            var lookDirection = direction;

            if (Input.GetKey(KeyCode.Space))
            {
                _characterMovementController.permSpeed(-1, false);
            } else
            {
                _characterMovementController.permSpeed(-1, true);
            }

            if (_shootingController.HasTarget)
                lookDirection = (_shootingController.TargetPosition - transform.position).normalized;
            _characterMovementController.MovementDirection = direction;
            _characterMovementController.LookDirection = lookDirection;

            if(_health <= 0f)
            {
                Destroy(gameObject);
            }
        }
        protected void OnTriggerEnter(Collider other)
        {
            if(LayerUtils.IsBullet(other.gameObject))
            {
                var bullet = other.gameObject.GetComponent<Bullet>();
                _health -= bullet.Damage;
                Destroy(other.gameObject);
            } else if (LayerUtils.IsPickUp(other.gameObject))
            {
                var pickUpType = other.gameObject.GetComponent<PickUpItem>()._type;
                var pickUp = other.gameObject.GetComponent<PickUpItem>();
                if(pickUpType == "weapon")
                {
                    pickUp = other.gameObject.GetComponent<PickUpWeapon>();
                    _hasDefaultWeapon = false;
                } else if (pickUpType == "boost")
                {
                    pickUp = other.gameObject.GetComponent<PickUpSpeedBoost>();
                }
                pickUp.PickUp(this);

                Destroy(other.gameObject);
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            _shootingController.SetWeapon(weapon, _hand);
        }

        public void SetSpeed(float speed, int timer = 0, bool escapingMode = false, bool end = false)
        {
            if (escapingMode)
            {
                _characterMovementController.permSpeed(speed, end);
            } else
            {
                _characterMovementController.Speed(speed, timer);
            }

        }

        public float GetHP()
        {
            return _health;
        }

        public bool hasDefaultWeapon()
        {
            return _hasDefaultWeapon;
        }

    }
}
