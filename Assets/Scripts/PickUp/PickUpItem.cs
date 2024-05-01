using LearnGame.Shooting;
using System;
using UnityEngine;

namespace LearnGame.PickUp
{
    public abstract class PickUpItem : MonoBehaviour
    {
        public event Action<PickUpItem> OnPickUp;

        public virtual void PickUp(BaseCharacter character)
        {
            OnPickUp?.Invoke(this);
        }

    }
}