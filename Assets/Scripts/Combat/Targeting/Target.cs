using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnDisabled;

        private void OnDisable() 
        {
            OnDisabled?.Invoke(this);
        }
    }
}

