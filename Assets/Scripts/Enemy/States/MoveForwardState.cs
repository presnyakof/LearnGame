﻿using LearnGame.FSM;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace LearnGame.Enemy.States
{
    public class MoveForwardState : BaseState
    {
        private readonly EnemyTarget _target;
        private readonly EnemyDirectionController _enemyDirectionController;

        private Vector3 _currentPoint;

        public MoveForwardState(EnemyTarget target, EnemyDirectionController enemyDirectionController)
        {
            _target = target;
            _enemyDirectionController = enemyDirectionController;
            StateID = "forward";
        }

        public override void Execute()
        {
            if(_target.Closest != null)
            { 
                Vector3 targetPosition = _target.Closest.transform.position;

                if(_currentPoint != targetPosition )
                {
                    _currentPoint = targetPosition;
                    _enemyDirectionController.UpdateMovementDirection(targetPosition);
                }
            }
        }
    }
}
