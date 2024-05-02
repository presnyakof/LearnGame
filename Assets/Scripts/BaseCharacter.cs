using UnityEngine;
using LearnGame.Movement;
using LearnGame.Shooting;
using LearnGame.PickUp;

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


        private IMovementDirectionSource _movementDirectionSource;
        private CharacterMovementController _characterMovementController;
        private ShootingController _shootingController;
        protected void Awake()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _movementDirectionSource = GetComponent<IMovementDirectionSource>();
            _shootingController = GetComponent<ShootingController>();
        }

        protected void Start()
        {
            SetWeapon(_baseWeaponPrefab);
        }

        protected void Update()
        {
            var direction = _movementDirectionSource.MovementDirection;
            var lookDirection = direction;

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
                Debug.Log(other.gameObject);
                var pickUpType = other.gameObject.GetComponent<PickUpItem>()._type;
                var pickUp = other.gameObject.GetComponent<PickUpItem>();
                if(pickUpType == "weapon")
                {
                    pickUp = other.gameObject.GetComponent<PickUpWeapon>();
                } else if (pickUpType == "speedboost")
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

        public void SetSpeed(float speed, int timer)
        {
            _characterMovementController.Speed(speed, timer);
        }
    }
}
