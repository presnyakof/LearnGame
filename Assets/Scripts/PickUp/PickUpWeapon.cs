using LearnGame.Shooting;
using UnityEngine;

namespace LearnGame.PickUp
{
    public class PickUpWeapon : PickUpItem
    {
        [SerializeField]
        private Weapon _WeaponPrefab;

        public override void PickUp(BaseCharacter character)
        {
            base.PickUp(character);
            character.SetWeapon(_WeaponPrefab);
        }
    }
}