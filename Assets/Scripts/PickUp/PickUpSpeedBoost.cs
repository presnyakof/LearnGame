using LearnGame.Shooting;
using UnityEngine;

namespace LearnGame.PickUp
{
    public class PickUpSpeedBoost : PickUpItem
    {
        [SerializeField]
        public float _multiplier = 1.5f;

        [SerializeField]
        public int _timer = 3;

        public override void PickUp(BaseCharacter character)
        {
            base.PickUp(character);
            character.SetSpeed(_multiplier, _timer);
        }
    }
}