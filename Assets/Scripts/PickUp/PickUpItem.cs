using LearnGame.Shooting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LearnGame.PickUp
{
    public abstract class PickUpItem : MonoBehaviour
    {
        public event Action<PickUpItem> OnPickUp;

        public string _type = null;

        public virtual void PickUp(BaseCharacter character)
        {
            OnPickUp?.Invoke(this);
        }

    }
}