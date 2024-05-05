using LearnGame.Enemy.States;
using UnityEditor;
using UnityEngine;

namespace LearnGame.Enemy
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField]
        private float _viewRadius = 20f;
        [SerializeField]
        private float _minHPPercent = 30f;
        [SerializeField]
        private float _escapeChance = 70f;
        [SerializeField]
        private float _escapingSpeedMultiplier = 1.5f;
        private EnemyTarget _target;
        private EnemyStateMachine _stateMachine;
        private float _enemyHP;

        protected void Awake()
        {
            var player = FindObjectOfType<PlayerCharacter>();
            var enemyDirectionController = GetComponent<EnemyDirectionController>();
            var navMesher = new NavMesher(transform);
            _target = new EnemyTarget(transform, player, _viewRadius);
            _stateMachine = new EnemyStateMachine(enemyDirectionController, navMesher, _target, _minHPPercent, _escapeChance);
        }

        protected void Update()
        {
            _enemyHP = gameObject.GetComponent<EnemyCharacter>().GetHP();
            _target.FindClosest(gameObject.GetComponent<EnemyCharacter>().hasDefaultWeapon());
            _stateMachine.Update(_enemyHP, gameObject.GetComponent<EnemyCharacter>()._maxHP);
            if(_stateMachine.returnCurrentStateID() == "escape")
            {
                gameObject.GetComponent<EnemyCharacter>().SetSpeed(_escapingSpeedMultiplier, 0, true, false);
            } else
            {
                gameObject.GetComponent<EnemyCharacter>().SetSpeed(_escapingSpeedMultiplier, 0, true, true);
            }
        }

        protected void OnDrawGizmos()
        {
            var cashedColor = Handles.color;
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.up, 1f);
            Handles.Label(gameObject.transform.position, "State: " + _stateMachine.returnCurrentStateID());
            Handles.color = cashedColor;
        }
    }
}