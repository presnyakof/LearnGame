using UnityEngine;

namespace LearnGame
{
    public class CharactersSpawnerController : MonoBehaviour
    {
        //Создан, чтобы чекать, сколько всего на карте персонажей и жив ли игрок
        //public int _AllCharacters {  get; private set; }

        private readonly Collider[] _colliders = new Collider[100];

        public int GetCharactersNumber() {;
            var size = Physics.OverlapSphereNonAlloc(new Vector3(0, 0, 0), 50f, _colliders, LayerUtils.CharactersMask);
            return size;
        }

        public bool isPlayerAlive()
        {
            var size = Physics.OverlapSphereNonAlloc(new Vector3(0, 0, 0), 50f, _colliders, LayerUtils.PlayerLayer) ;
            return size > 0;
        }


    }
}