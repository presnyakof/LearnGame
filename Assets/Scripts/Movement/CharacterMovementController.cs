using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace LearnGame.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementController : MonoBehaviour
    {
        private static readonly float SqrEpsilon = Mathf.Epsilon * Mathf.Epsilon;
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _accelerationTimes = 1.5f;
        [SerializeField]
        private float _maxRadiansDelta = 10f;
        public Vector3 MovementDirection { get; set; }
        public Vector3 LookDirection { get; set; }
        private CharacterController _characterController;
        private float _timer; // сколько буст действует
        private float _time; // сколько прошло
        private bool _boosted;
        private float _boostedSpeed;

        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        protected void Update()
        {
            Translate(Input.GetKey(KeyCode.Space));

            if(_boosted)
            {
                _time += Time.deltaTime;
                if (_time > _timer)
                {
                    _boosted = false;
                    _time = 0;
                }
            }


            if(_maxRadiansDelta > 0 && LookDirection != Vector3.zero)
            {
                Rotate();
            }
        }
        private void Translate(bool acceleration)
        {
            float speed = _speed;
            if (_boosted) {
                speed = _boostedSpeed;
            } else if (acceleration)
            {
                speed = _speed * _accelerationTimes;
            }
            var delta = MovementDirection * speed * Time.deltaTime;
            _characterController.Move(delta);
        }
        private void Rotate()
        {
            var currentLookDirection = transform.rotation * Vector3.forward;
            float sqrMagnitude = (currentLookDirection - LookDirection).sqrMagnitude;

            if(sqrMagnitude > SqrEpsilon)
            {
                var newRotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(LookDirection, Vector3.up),
                    _maxRadiansDelta * Time.deltaTime);

                transform.rotation = newRotation;
            }
        }

        public void Speed(float speed, int timer = 0)
        {
            if (timer > 0)
            {
                _boosted = true;
                _timer = timer;
            }
            _boostedSpeed = _speed * speed;
        }
    }
}