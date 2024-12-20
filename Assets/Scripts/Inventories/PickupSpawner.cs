using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] GameObject pickup;
        [SerializeField] Item item;
        [SerializeField] int amount = 1;
        public bool isPicked = false;

        private void Start() 
        {
            if(!isPicked)
            {
                GameObject pickupInstantion= Instantiate(pickup,gameObject.transform);
                pickupInstantion.GetComponent<Pickup>().spawner = this;
                pickupInstantion.GetComponent<Pickup>().item = item;
                pickupInstantion.GetComponent<Pickup>().amount = amount;
            }
        }

        public object CaptureState()
        {
            return isPicked;
        }

        public void RestoreState(object state)
        {
            if(state == null) return;
            isPicked = (bool)state;
        }
    }

}
