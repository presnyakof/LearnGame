using LearnGame.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LearnGame.Enemy.States
{
    public class EnemyStateMachine : BaseStateMachine
    {
        private const float NavMeshTurnOffDistance = 10;
        readonly Func<float, float, float,float, bool> Decision = delegate (float minHP, float escapeChance, float HP, float maxHP)
        {
            var result = UnityEngine.Random.value;
            if(HP <= minHP/100*maxHP && result <= escapeChance/100)
            {
                return true;
            } else
            {
                return false;
            }
        };
        public EnemyStateMachine(EnemyDirectionController enemyDirectionController, NavMesher navMesher, 
            EnemyTarget target, float minHP, float escapeChance) 
        {
            var idleState = new IdleState();
            var findWayState = new FindWayState(target, navMesher, enemyDirectionController);
            var moveForwardState = new MoveForwardState(target, enemyDirectionController);
            var escapeState = new EscapeState(target, enemyDirectionController);
            
            SetInitialState(idleState);

            AddState(state: idleState, transitions: new List<Transition>
            {
                new Transition(findWayState, () => target.DistanceToClosestFromAgent() > NavMeshTurnOffDistance),
                new Transition(moveForwardState, () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance),
                new Transition(escapeState, () => Decision(minHP, escapeChance, _hp, _maxhp))
            });

            AddState(state: findWayState, transitions: new List<Transition>
            {
                new Transition(idleState, () => target.Closest == null),
                new Transition(moveForwardState, () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance)
            });

            AddState(state: moveForwardState, transitions: new List<Transition>
            {
                new Transition(idleState, () => target.Closest == null),
                new Transition(findWayState, () => target.DistanceToClosestFromAgent() > NavMeshTurnOffDistance),
                new Transition(escapeState, () => Decision(minHP, escapeChance, _hp, _maxhp))
            });


            AddState(state: escapeState, transitions: new List<Transition>
            {
                new Transition(idleState, () => target.Closest == null)
            });
        }
    }
}
