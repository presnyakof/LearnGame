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
        private float _accelerationTimes { get; set; } = 1.5f;
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
            Translate();
            if(_maxRadiansDelta > 0 && LookDirection != Vector3.zero)
            {
                Rotate();
            }
        }
        private void Translate()
        {
            float speed = _speed;
            //Debug.Log("_boosted : " + _boosted + "; _timer : " + _timer + "; _time = " + _time + "; _boostedSpeed : " + _boostedSpeed);
            if (_boosted && _timer > 0 && _time > _timer) {
                //speed = _boostedSpeed;
                _boosted = false;
                _timer = 0;
                _time = 0;
            } else 
            if (_boosted && (_time <= _timer || _timer == 0))
            {
                speed = _boostedSpeed;
                _time += Time.deltaTime;
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
                _time = 0;
                _boostedSpeed = _speed * speed;
                Debug.Log("_boosted : " + _boosted + "; _timer : " + _timer + "; _time = " + _time + "; _boostedSpeed : " + _boostedSpeed);
            }
        }

        public void permSpeed (float speedx = -1, bool enabled = true)
        {
            if (!enabled && !_boosted)
            {
                _boosted = true;
                if (speedx == -1) 
                    _boostedSpeed = _accelerationTimes * _speed;
                else
                    _boostedSpeed = _speed * speedx;
            } else if (_timer == 0)
            {
                _boosted = false;
                _boostedSpeed = _speed;
            }
        }
    }
}