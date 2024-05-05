using UnityEngine;

namespace LearnGame.Enemy
{
    public class EnemyTarget
    {
        public GameObject Closest {  get; private set; }

        private readonly float _viewRadius;

        private readonly Transform _agentTransform;

        private readonly PlayerCharacter _player;

        private readonly Collider[] _colliders = new Collider[100];
        public EnemyTarget(Transform agent, PlayerCharacter player, float viewRadius)
        {
            _agentTransform = agent;
            _player = player;
            _viewRadius = viewRadius;
        } 
        public void FindClosest(bool hasDefaultWeapon)
        {
            float minDistance = float.MaxValue;
            int count;

            if(hasDefaultWeapon)
            {
                count = FindAllTargets(LayerUtils.PickUpsMask);
            } else if(_colliders.Length == 0)
            {
                count = FindAllTargets(LayerUtils.PickUpsMask);
            } else
            {
                count = FindAllTargets(LayerUtils.CharactersMask);
            }
            for (int i = 0; i < count; i++)
            {
                var go = _colliders[i].gameObject;
                if (go == _agentTransform.gameObject) continue;

                var distance = DistanceFromAgentTo(go);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    Closest = go;
                }
            }

            if(_player != null && DistanceFromAgentTo(_player.gameObject) < minDistance && !hasDefaultWeapon) Closest = _player.gameObject;
        }

        public float DistanceToClosestFromAgent()
        {
            if (Closest != null)
                DistanceFromAgentTo(Closest);

            return 0;
        }

        private int FindAllTargets(int layerMask)
        {
            var size = Physics.OverlapSphereNonAlloc(_agentTransform.position, _viewRadius, _colliders, layerMask);
            return size;
        }

        private float DistanceFromAgentTo(GameObject go) => (_agentTransform.position - go.transform.position).magnitude;
    }
}
