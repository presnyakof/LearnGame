using LearnGame.FSM;
using UnityEngine;

namespace LearnGame.Enemy.States
{
    public class EscapeState : BaseState
    {
        private readonly EnemyTarget _target;
        private readonly EnemyDirectionController _enemyDirectionController;

        private Vector3 _currentPoint;

        public EscapeState(EnemyTarget target, EnemyDirectionController enemyDirectionController)
        {
            _target = target;
            _enemyDirectionController = enemyDirectionController;
            StateID = "escape";
        }

        public override void Execute()
        {
            Vector3 targetPosition = _target.Closest.transform.position;
            _currentPoint = (targetPosition+ new Vector3(-1,0,-1))*-100;
            _enemyDirectionController.UpdateMovementDirection(_currentPoint);
        }
    }
}
